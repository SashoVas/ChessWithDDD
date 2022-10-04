using Domain.Events;
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
                throw new Exception("More pieces than squeres");
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
                throw new Exception("This is not the player's turn");
            }
            var board = ConstructBoard();
            var piece = board[startPosition.Row, startPosition.Col];
            if (piece is null)
            {
                throw new Exception("Start position should be a piece");
            }
            if (board[move.Row,move.Col] is not null && board[move.Row, move.Col].Color==piece.Color)
            {
                throw new Exception("There cannot be more then one pieces on a squere");
            }
            foreach (var movePatter in piece.Moves)
            {
                var moves = GetAvelableMoves(piece.Position, board, movePatter.RowChange, movePatter.ColChange, movePatter.IsRepeatable, movePatter.SwapDirections, piece.Color);
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
                }
            }
            ValidateChecks(piece,board);
            IsWhiteOnTurn = !IsWhiteOnTurn;
        }
        public void AddPiece(Piece piece)
        {
            if (Pieces.Any(p=>p.Position==piece.Position))
            {
                throw new Exception("There cannot be more then one pieces on a squere");
            }
            Pieces.Add(piece);
            AddEvent(new AddPieceToBoardDomainEvent(piece));
        }
        public void AddPieces(List<Piece> pieces) 
            => pieces.ForEach(p => AddPiece(p));
        private void ValidateChecks(Piece piece,Piece[,] board)
        {
            foreach (var pieceToCheckForCheck in Pieces)
            {
                if (IsInCheck(pieceToCheckForCheck, board))
                {
                    if (pieceToCheckForCheck.Color != piece.Color)
                    {
                        throw new Exception("You are in check");
                    }
                }
            }
        }
        private bool IsInCheck(Piece piece, Piece[,]board)
        {
            foreach (var movePatter in piece.Moves)
            {
                var moves = GetAvelableMoves(piece.Position, board, movePatter.RowChange, movePatter.ColChange, movePatter.IsRepeatable, movePatter.SwapDirections, piece.Color);
                if (moves.Any(m =>board[m.Row, m.Col]is not null && board[m.Row,m.Col].Name=="king" && board[m.Row, m.Col].Color!=piece.Color))
                {
                    return true;
                }
                
            }
            return false;
        }
        private bool IsInMate()
        {
            throw new NotImplementedException();
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
        private List<PiecePosition> GetAvelableMoves(PiecePosition startPosition, Piece[,] board, int rowChange, int colChange, bool IsRepeatable, bool SwapDirections,PieceColor color)
        {
            List<PiecePosition> result = new List<PiecePosition>();
            if (IsRepeatable)
            {
                if (SwapDirections)
                {
                    AddPositions(startPosition.Row, startPosition.Col, result, -rowChange, colChange, board, color);
                    AddPositions(startPosition.Row, startPosition.Col, result, rowChange, -colChange, board, color);
                    AddPositions(startPosition.Row, startPosition.Col, result, -rowChange, -colChange, board, color);
                }
                AddPositions(startPosition.Row, startPosition.Col, result, rowChange, colChange, board, color);
            }
            else
            {
                if (SwapDirections)
                {
                    AddPosition(startPosition, result, -rowChange, colChange, board,color);
                    AddPosition(startPosition, result, rowChange, -colChange, board,color);
                    AddPosition(startPosition, result, -rowChange, -colChange, board,color);
                }
                AddPosition(startPosition, result, rowChange, colChange, board, color);
            }
            return result;
        }
        private void AddPosition(PiecePosition startPosition, List<PiecePosition> result, int rowChange, int colChange, Piece[,] board, PieceColor color)
        {
            int currentRow = startPosition.Row + rowChange;
            int currentCol = startPosition.Col + colChange;
            if (currentRow >= DomainConstants.DefaultBoardRows || currentCol >= DomainConstants.DefaultBoardCols || currentRow < 0 || currentCol <0
                ||(board[currentRow,currentCol] is not null && board[currentRow, currentCol].Color==color))
            {
                return;
            }
            result.Add(new PiecePosition(currentRow, currentCol));
        }
        private void AddPositions(int currentRow, int currentCol, List<PiecePosition> result, int rowChange, int colChange, Piece[,] board, PieceColor color)
        {
            currentRow += rowChange;
            currentCol += colChange;
            while (currentRow < DomainConstants.DefaultBoardRows || currentCol < DomainConstants.DefaultBoardCols || currentRow >=0 || currentCol >= 0)
            {
                if (board[currentRow,currentCol] is not null)
                {
                    if (board[currentRow, currentCol].Color!=color)
                    {
                        result.Add(new PiecePosition(currentRow, currentCol));
                    }
                    break;
                }
                currentRow += rowChange;
                currentCol += colChange;
            }
        }
    }
}
