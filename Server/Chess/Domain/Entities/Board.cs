using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Entities
{
    public class Board : AggregateRoot
    {
        public List<Piece> Pieces { get;  }
        public FenIdentifier Fen { get; private set; }
        public Guid WhitePlayerId { get; private init; }
        public Guid BlackPlayerId { get; private init; }
        public bool IsWhiteOnTurn { get; private set; }
        public BoardClock BoardClock { get; private set; }
        private Board() : base(Guid.NewGuid())
        {
        }
        
        internal Board(Guid Id,Guid whitePlayerId,Guid blackPlayerId, BoardClock boardClock) :base(Id)
        {
            if (whitePlayerId == Guid.Empty)
            {
                throw new InvalidPlayerIdException(whitePlayerId);
            }
            if (blackPlayerId == Guid.Empty)
            {
                throw new InvalidPlayerIdException(blackPlayerId);
            }
            Pieces = new List<Piece>();
            Fen = DomainConstants.EmptyBoardFen;
            IsWhiteOnTurn = true;
            WhitePlayerId = whitePlayerId;
            BlackPlayerId = blackPlayerId;
            BoardClock = boardClock;
        }
        internal Board(Guid Id, List<Piece> pieces, Guid whitePlayerId, Guid blackPlayerId,BoardClock boardClock) : base(Id)
        {
            if (pieces.Count() > DomainConstants.DefaultBoardRows * DomainConstants.DefaultBoardCols)
            {
                throw new TooManyPiecesException(null);
            }
            if (whitePlayerId == Guid.Empty)
            {
                throw new InvalidPlayerIdException(whitePlayerId);
            }
            if (blackPlayerId==Guid.Empty)
            {
                throw new InvalidPlayerIdException(blackPlayerId);
            }
            Pieces = pieces;
            Fen = ConstructBoard();
            IsWhiteOnTurn = true;
            WhitePlayerId = whitePlayerId;
            BlackPlayerId = blackPlayerId;
            BoardClock = boardClock;
        }
        public void MakeAMove(PiecePosition startPosition,PiecePosition move,Guid playerId)
        {
            if (playerId ==Guid.Empty)
            {
                throw new InvalidPlayerIdException(playerId);
            }
            if ((IsWhiteOnTurn &&WhitePlayerId!=playerId)||(!IsWhiteOnTurn && BlackPlayerId!=playerId))
            {
                throw new WrongTurnException();
            }
            var board = ConstructBoard();
            var piece = board[startPosition.Row, startPosition.Col];
            ValidatePieceMoves(piece,startPosition,move,board);

            foreach (var movePatter in piece.Moves)
            {
                var moves = GetAvelableMovesThatLeadToATarget(piece.Position, board, movePatter.RowChange, movePatter.ColChange, movePatter.IsRepeatable, movePatter.SwapDirections, piece.Color,move);
                if (moves.Contains(move))
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
                    BoardClock.DecrementTimer(IsWhiteOnTurn?PieceColor.White:PieceColor.Black,DateTime.UtcNow);
                    return;
                }
            }
            throw new PieceDoesntHaveThatMoveException();
        }
        public void AddPiece(Piece piece,Guid playerId)
        {
            if (playerId == Guid.Empty)
            {
                throw new InvalidPlayerIdException(playerId);
            }
            if (piece.Color==PieceColor.White && WhitePlayerId!=playerId)
            {
                throw new ForbiddenPiceAddingException(playerId,piece);
            }
            if (piece.Color == PieceColor.Black && BlackPlayerId != playerId)
            {
                throw new ForbiddenPiceAddingException(playerId, piece);
            }
            if (Pieces.Any(p=>p.Position==piece.Position))
            {
                throw new TooManyPiecesException(piece.Position);
            }
            Pieces.Add(piece);
            AddEvent(new AddPieceToBoardDomainEvent(piece));
        }
        public void AddPieces(List<Piece> pieces,Guid playerId) 
            => pieces.ForEach(p => AddPiece(p,playerId));
        private void ValidatePieceMoves(Piece piece,PiecePosition startPosition, PiecePosition move, Piece[,]board)
        {
            if (piece is null)
            {
                throw new PositionShouldBeAPieceException(startPosition);
            }
            if ((piece.Color == PieceColor.White && !IsWhiteOnTurn) || (piece.Color == PieceColor.Black && IsWhiteOnTurn))
            {
                throw new CanNotMoveOponentPiecesException();
            }
            if (board[move.Row, move.Col] is not null && board[move.Row, move.Col].Color == piece.Color)
            {
                throw new TooManyPiecesException(move);
            }
        }
        private void ValidateChecks(Piece piece,Piece[,] board)
        {
            bool playerInCheck = false;
            bool[,] checkedPositions = new bool[DomainConstants.DefaultBoardRows, DomainConstants.DefaultBoardCols];
            var listOfMovesThatPutTheEnemyInCheck = new List<HashSet<PiecePosition>>();
            Piece whiteKing = Pieces.First(p => p.Name == DomainConstants.KingName && p.Color == PieceColor.White);
            Piece blackKing = Pieces.First(p=>p.Name== DomainConstants.KingName && p.Color==PieceColor.Black);
            foreach (var pieceToCheckForCheck in Pieces)
            {
                var moves = GetCheckedPositions(pieceToCheckForCheck, board, checkedPositions, pieceToCheckForCheck.Color, pieceToCheckForCheck.Color!=PieceColor.White?whiteKing.Position:blackKing.Position);
                if (moves is not null)
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
                if (piece.Color == color)
                {
                    CheckIteratedPositions(checkedPositions, moves);
                }
                if (moves.Any(m =>board[m.Row, m.Col]is not null && board[m.Row,m.Col].Name== DomainConstants.KingName && board[m.Row, m.Col].Color!=piece.Color))
                {
                    return moves;
                }
            }
            return null;
        }
        private void CheckIteratedPositions(bool[,] checkedPositions,HashSet<PiecePosition>moves)
        {
            foreach (var iteratedPosition in moves)
            {
                checkedPositions[iteratedPosition.Row, iteratedPosition.Col] = true;
            }
        }
        private bool IsInMate(Piece king,bool[,] checkedPositions, Piece[,]board, List<HashSet<PiecePosition>> listOfMovesThatPutTheEnemyInCheck)
        {
            if (EscapeMateWithKingMove(king,checkedPositions,board)|| EscapeMateByBlockingOrTakingAttackingPiece(king,board,listOfMovesThatPutTheEnemyInCheck)) 
            {
                return false; 
            }
            return true;
        }
        private bool EscapeMateWithKingMove(Piece king, bool[,] checkedPositions, Piece[,] board)
        {
            foreach (var movePattern in king.Moves)
            {
                var moves = GetAvelableMoves(king.Position, board, movePattern.RowChange, movePattern.ColChange, movePattern.IsRepeatable, movePattern.SwapDirections, king.Color);
                if (moves.Any(m => !checkedPositions[m.Row, m.Col]))
                {
                    return true;
                }
            }
            return false;
        }
        private bool EscapeMateByBlockingOrTakingAttackingPiece(Piece king, Piece[,] board, List<HashSet<PiecePosition>> listOfMovesThatPutTheEnemyInCheck)
        {
            foreach (var piece in Pieces)
            {
                if (piece.Color != king.Color || piece.Name.Name == DomainConstants.KingName)
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
                            if (index >= listOfMovesThatPutTheEnemyInCheck.Count())
                            {
                                return true;
                            }
                            currentCheck = listOfMovesThatPutTheEnemyInCheck[index];
                        }
                    }
                }
            }
            return false;
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
        private HashSet<PiecePosition> GetAvelableMovesThatLeadToATarget(PiecePosition startPosition,
            Piece[,] board,
            int rowChange,
            int colChange,
            bool IsRepeatable,
            bool SwapDirections,
            PieceColor color,
            PiecePosition target)
        {
            var result = new HashSet<PiecePosition>();
            if (IsRepeatable)
            {
                CallPositionAddingFunction(startPosition,board,rowChange,colChange,result,SwapDirections,color,target,AddPositions);
            }
            else
            {
                CallPositionAddingFunction(startPosition, board, rowChange, colChange, result, SwapDirections, color, target, AddPosition);
            }
            return result;
        }
        private void CallPositionAddingFunction(PiecePosition startPosition,
            Piece[,] board,
            int rowChange,
            int colChange,
            HashSet<PiecePosition>result,
            bool SwapDirections,
            PieceColor color,
            PiecePosition target,
            Func<PiecePosition , int , int , Piece[,] , PieceColor , PiecePosition , HashSet<PiecePosition>,bool>addingFunction)
        {
            if (SwapDirections)
            {
                if (addingFunction(startPosition, -rowChange, colChange, board, color, target, result))
                {
                    return;
                }
                result.Clear();
                if (addingFunction(startPosition, rowChange, -colChange, board, color, target, result))
                {
                    return ;
                }
                result.Clear();
                if (addingFunction(startPosition, -rowChange, -colChange, board, color, target, result))
                {
                    return ;
                }
                result.Clear();
            }
            if (addingFunction(startPosition, rowChange, colChange, board, color, target, result))
            {
                return ;
            }
            result.Clear();
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
                    AddPositions(startPosition, -rowChange, colChange, board, color, null, result);
                    AddPositions(startPosition, rowChange, -colChange, board, color, null, result);
                    AddPositions(startPosition, -rowChange, -colChange, board, color, null, result);
                }
                AddPositions(startPosition, rowChange, colChange, board, color, null, result);
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
        private bool AddPosition(PiecePosition startPosition, int rowChange, int colChange, Piece[,] board, PieceColor color,PiecePosition target, ICollection<PiecePosition>result)
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
        private bool AddPositions(PiecePosition startPosition, int rowChange, int colChange, Piece[,] board, PieceColor color,PiecePosition target, ICollection<PiecePosition>result)
        {
            int currentRow = startPosition.Row + rowChange;
            int currentCol = startPosition.Col + colChange;
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
