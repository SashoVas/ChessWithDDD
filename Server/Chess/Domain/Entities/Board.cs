using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Entities
{
    public sealed class Board : AggregateRoot
    {
        public List<Piece> Pieces { get;  }
        public FenIdentifier Fen { get; private set; }
        public Guid WhitePlayerId { get; private init; }
        public Guid BlackPlayerId { get; private init; }
        public bool IsWhiteOnTurn { get; private set; }
        internal Board(Guid Id,Guid whitePlayerId,Guid blackPlayerId):base(Id)
        {
            Pieces = new List<Piece>();
            Fen = DomainConstants.EmptyBoardFen;
            IsWhiteOnTurn = true;
            WhitePlayerId = whitePlayerId;
            BlackPlayerId = blackPlayerId;
        }

        internal Board(Guid Id, List<Piece> pieces, Guid whitePlayerId, Guid blackPlayerId) : base(Id)
        {
            if (pieces.Count() > DomainConstants.DefaultBoardRows * DomainConstants.DefaultBoardCols)
            {
                throw new TooManyPiecesException(null);
            }
            Pieces = pieces;
            Fen = ConstructBoard();
            IsWhiteOnTurn = true;
            WhitePlayerId = whitePlayerId;
            BlackPlayerId = blackPlayerId;
        }
        public void MakeAMove(PiecePosition startPosition,PiecePosition move,Guid playerId)
        {
            if ((IsWhiteOnTurn &&WhitePlayerId!=playerId)||(!IsWhiteOnTurn && BlackPlayerId!=playerId))
            {
                throw new WrongTurnException();
            }
            var board = ConstructBoard();
            var piece = board[startPosition.Row, startPosition.Col];
            if (piece is null)
            {
                throw new PositionShouldBeAPieceException(startPosition);
            }
            if ((piece.Color==PieceColor.White&&!IsWhiteOnTurn)|| (piece.Color == PieceColor.Black && IsWhiteOnTurn))
            {
                throw new CanNotMoveOponentPiecesException();
            }
            if (board[move.Row,move.Col] is not null && board[move.Row, move.Col].Color==piece.Color)
            {
                throw new TooManyPiecesException(move);
            }
            
            
            foreach (var movePatter in piece.Moves)
            {
                var moves = GetAvelableMovesThatLeadToATarget(piece.Position, board, movePatter.RowChange, movePatter.ColChange, movePatter.IsRepeatable, movePatter.SwapDirections, piece.Color,move);
                if (moves
                    .Any(m=>m==move))
                {
                    Piece takenPiece = null;
                    if (board[move.Row, move.Col] is not null)
                    {
                        board[move.Row, move.Col].TakePiece();
                        takenPiece = board[move.Row, move.Col];
                    }
                    board[startPosition.Row, startPosition.Col] = null;
                    board[move.Row, move.Col] = piece;
                    piece.MakeAMove(move);
                    Fen =board;
                    AddEvent(new MakeAMoveBoardDomainEvent(piece,takenPiece, startPosition, move));
                    ValidateChecks(piece,board);
                    IsWhiteOnTurn = !IsWhiteOnTurn;
                    return;
                }
            }
            throw new PieceDoesntHaveThatMoveException();
        }
        public void AddPiece(Piece piece)
        {
            if (Pieces.Any(p=>p.Position==piece.Position))
            {
                throw new TooManyPiecesException(piece.Position);
            }
            Pieces.Add(piece);
            AddEvent(new AddPieceToBoardDomainEvent(piece));
        }
        public void AddPieces(List<Piece> pieces) 
            => pieces.ForEach(p => AddPiece(p));
        private void ValidateChecks(Piece piece,Piece[,] board)
        {
            bool playerInCheck = false;
            bool[,] checkedPositions = new bool[DomainConstants.DefaultBoardRows, DomainConstants.DefaultBoardCols];
            var listOfMovesThatPutTheEnemyInCheck = new List<HashSet<PiecePosition>>();
            Piece whiteKing = Pieces.First(p=>p.Name=="king"&&p.Color==PieceColor.White);
            Piece blackKing = Pieces.First(p=>p.Name=="king"&&p.Color==PieceColor.Black);
            foreach (var pieceToCheckForCheck in Pieces)
            {
                var moves = GetCheckedPositions(pieceToCheckForCheck, board, checkedPositions, pieceToCheckForCheck.Color, pieceToCheckForCheck.Color!=PieceColor.White?whiteKing.Position:blackKing.Position);
                if (moves.Count()>0)
                {
                    playerInCheck = true;
                    if (pieceToCheckForCheck.Color != piece.Color)
                    {
                        throw new InvalidMoveWhenCheckedException();
                    }
                    listOfMovesThatPutTheEnemyInCheck.Add(moves);
                }
            }
            if (playerInCheck && IsInMate(piece.Color==PieceColor.White?blackKing:whiteKing,checkedPositions,board,listOfMovesThatPutTheEnemyInCheck))
            {
                throw new NotImplementedException();
            }
        }
        private HashSet<PiecePosition> GetCheckedPositions(Piece piece, Piece[,] board, bool[,] checkedPositions, PieceColor color,PiecePosition target)
        {
            foreach (var movePatter in piece.Moves)
            {
                var moves = GetAvelableMovesThatLeadToATarget(piece.Position, board, movePatter.RowChange, movePatter.ColChange, movePatter.IsRepeatable, movePatter.SwapDirections, piece.Color,target);
                if (moves.Any(m =>board[m.Row, m.Col]is not null && board[m.Row,m.Col].Name=="king" && board[m.Row, m.Col].Color!=piece.Color))
                {
                    if (piece.Color == color)
                    {
                        moves.ForEach(m => checkedPositions[m.Row, m.Col] = true);
                    }
                    return moves.ToHashSet();
                }
                if (piece.Color==color)
                {
                    moves.ForEach(m => checkedPositions[m.Row, m.Col] = true);
                }
            }
            return new HashSet<PiecePosition>();
        }
        private bool IsInMate(Piece king,bool[,] checkedPositions, Piece[,]board, List<HashSet<PiecePosition>> listOfMovesThatPutTheEnemyInCheck)
        {
            foreach (var movePattern in king.Moves)
            {
                var moves = GetAvelableMoves(king.Position, board, movePattern.RowChange, movePattern.ColChange, movePattern.IsRepeatable, movePattern.SwapDirections, king.Color);
                if (moves.Any(m => !checkedPositions[m.Row,m.Col]))
                {
                    return false;
                }
            }
            foreach (var piece in Pieces)
            {
                if (piece.Color!=king.Color || piece.Name.Name=="king")
                {
                    continue;
                }
                foreach (var movePattern in piece.Moves)
                {
                    var moves = GetAvelableMoves(piece.Position, board, movePattern.RowChange, movePattern.ColChange, movePattern.IsRepeatable, movePattern.SwapDirections, piece.Color);
                    foreach (var move in moves)
                    {
                        int index = 0;
                        var currentCheck = listOfMovesThatPutTheEnemyInCheck[index];
                        while (currentCheck.Contains(move))
                        {
                            index++;
                            if (index>= listOfMovesThatPutTheEnemyInCheck.Count())
                            {
                                return false;
                            }
                            currentCheck = listOfMovesThatPutTheEnemyInCheck[index];
                        }
                    }
                }
            }
            return true;
        }
        private Piece[,] ConstructBoard()
        {
            Piece[,] board = new Piece[DomainConstants.DefaultBoardRows, DomainConstants.DefaultBoardCols];
            foreach (var piece in Pieces)
            {
                if (piece.IsTaken)
                {
                    continue;
                }
                board[piece.Position.Row, piece.Position.Col] = piece;
            }
            return board;
        }
        private List<PiecePosition> GetAvelableMovesThatLeadToATarget(PiecePosition startPosition,
            Piece[,] board,
            int rowChange,
            int colChange,
            bool IsRepeatable,
            bool SwapDirections,
            PieceColor color,
            PiecePosition target)
        {
            var result = new List<PiecePosition>();
            if (IsRepeatable)
            {
                if (SwapDirections)
                {
                    if (AddPositions(startPosition.Row, startPosition.Col, -rowChange, colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                    if (AddPositions(startPosition.Row, startPosition.Col, rowChange, -colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                    if (AddPositions(startPosition.Row, startPosition.Col, -rowChange, -colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                }
                if (AddPositions(startPosition.Row, startPosition.Col, rowChange, colChange, board, color, target, result))
                {
                    return result;
                }
                result.Clear();
            }
            else
            {
                if (SwapDirections)
                {
                    if (AddPosition(startPosition, -rowChange, colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                    if (AddPosition(startPosition, rowChange, -colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                    if (AddPosition(startPosition, -rowChange, -colChange, board, color, target, result))
                    {
                        return result;
                    }
                    result.Clear();
                }
                if (AddPosition(startPosition, rowChange, colChange, board, color, target, result))
                {
                    return result;
                }
                result.Clear();
            }
            return result;
        }
        private List<PiecePosition> GetAvelableMoves(PiecePosition startPosition,
            Piece[,] board,
            int rowChange,
            int colChange,
            bool IsRepeatable,
            bool SwapDirections,
            PieceColor color)
        {
            var result = new List<PiecePosition>();
            if (IsRepeatable)
            {
                if (SwapDirections)
                {
                    AddPositions(startPosition.Row, startPosition.Col, -rowChange, colChange, board, color, null, result);
                    AddPositions(startPosition.Row, startPosition.Col, rowChange, -colChange, board, color, null, result);
                    AddPositions(startPosition.Row, startPosition.Col, -rowChange, -colChange, board, color, null, result);
                }
                AddPositions(startPosition.Row, startPosition.Col, rowChange, colChange, board, color, null, result);
            }
            else
            {
                if (SwapDirections)
                {
                    AddPosition(startPosition, -rowChange, colChange, board, color, null, result);
                    AddPosition(startPosition, rowChange, -colChange, board, color, null, result);
                    AddPosition(startPosition, -rowChange, -colChange, board, color, null, result);
                }
                AddPosition(startPosition, rowChange, colChange, board, color, null, result);
            }
            return result;
        }
        private bool AddPosition(PiecePosition startPosition, int rowChange, int colChange, Piece[,] board, PieceColor color,PiecePosition target, List<PiecePosition>result)
        {
            int currentRow = startPosition.Row + rowChange;
            int currentCol = startPosition.Col + colChange;
            if (currentRow >= DomainConstants.DefaultBoardRows || currentCol >= DomainConstants.DefaultBoardCols || currentRow < 0 || currentCol <0
                ||(board[currentRow,currentCol] is not null && board[currentRow, currentCol].Color==color))
            {
                return false;
            }
            result.Add( new PiecePosition(currentRow, currentCol));
            if (target is not null && currentRow==target.Row &&currentCol==target.Col)
            {
                return true;
            }
            return false;
        }
        private bool AddPositions(int currentRow, int currentCol, int rowChange, int colChange, Piece[,] board, PieceColor color,PiecePosition target, List<PiecePosition>result)
        {
            
            currentRow += rowChange;
            currentCol += colChange;
            while (currentRow < DomainConstants.DefaultBoardRows && currentCol < DomainConstants.DefaultBoardCols && currentRow >=0 && currentCol >= 0)
            {
                
                if (board[currentRow,currentCol] is not null)
                {
                    
                    if (board[currentRow, currentCol].Color!=color)
                    {
                        result.Add(new PiecePosition(currentRow, currentCol));
                    }
                    if (target is not null && currentRow == target.Row && currentCol == target.Col)
                    {
                        return true;
                    }
                    break;
                }
                else
                {
                    result.Add(new PiecePosition(currentRow, currentCol));
                }
                if (target is not null && currentRow==target.Row && currentCol==target.Col)
                {
                    return true;
                }
                currentRow += rowChange;
                currentCol += colChange;
            }
            return false;
        }
    }
}
