using Domain.Events;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Entities
{
    public sealed class Board : AggregateRoot
    {
        public List<Piece> Pieces { get;  }
        public FenIdentifier Fen { get;private set; }
        internal Board(Guid Id):base(Id)
        {
            Pieces = new List<Piece>();
            Fen = DomainConstants.EmptyBoardFen;
        }

        internal Board(Guid Id, List<Piece> pieces) : base(Id)
        {
            if (pieces.Count() > DomainConstants.DefaultBoardRows * DomainConstants.DefaultBoardCols)
            {
                throw new Exception("More pieces than squeres");
            }
            Pieces = pieces;
            Fen = ConstructBoard();
        }
        public void MakeAMove(PiecePosition startPosition,PiecePosition move)
        {
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
