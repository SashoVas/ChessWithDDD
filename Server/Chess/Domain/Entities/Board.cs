using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Entities
{
    public sealed class Board : Entity
    {
        public List<Piece> Pieces { get;  }
        public FenIdentifier Fen { get;private set; }
        internal Board(Guid Id):base(Id)
        {
            Pieces = new List<Piece>();
            Fen = "8/8/8/8/8/8/8/8";
        }

        internal Board(Guid Id, List<Piece> pieces) : base(Id)
        {
            if (pieces.Count()>8*8)
            {
                throw new Exception("More pieces than squeres");
            }
            Pieces = pieces;
            Fen = ConstructBoard();
        }
        public void MakeAMove(Guid pieceId,PiecePosition move)
        {
            var board = ConstructBoard();
            var piece=Pieces.FirstOrDefault(p => p.Id == pieceId);
            if (board[move.Row,move.Col] is not null && board[move.Row, move.Col].Color==piece.Color)
            {
                throw new Exception("There cannot be more then one pieces on a squere");
            }
            foreach (var movePatter in piece.Moves)
            {
                if (GetAvelableMoves(piece.Position,board,movePatter.RowChange,movePatter.ColChange,movePatter.IsRepeatable,movePatter.SwapDirections,piece.Color)
                    .Any(m=>m==move))
                {
                    if (board[move.Row, move.Col] is not null)
                    {
                        board[move.Row, move.Col].TakePiece();
                    }
                    board[piece.Position.Row, piece.Position.Col] = null;
                    board[move.Row, move.Col] = piece;
                    piece.MakeAMove(move);
                    Fen =board;
                }
            }
        }
        private Piece[,] ConstructBoard()
        {
            Piece[,] board = new Piece[8,8];
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
            if (!(currentRow >= 8 || currentCol >= 8 || currentRow < 8 || currentCol < 8)
                ||(board[currentRow,currentCol] is not null && board[currentRow, currentCol].Color==color))
            {
                return;
            }
            result.Append(new PiecePosition(currentRow, currentCol));
        }
        private void AddPositions(int currentRow, int currentCol, List<PiecePosition> result, int rowChange, int colChange, Piece[,] board, PieceColor color)
        {
            currentRow += rowChange;
            currentCol += colChange;
            while (currentRow < 8 || currentCol < 8 || currentRow >=0 || currentCol >= 0)
            {
                if (board[currentRow,currentCol] is not null)
                {
                    if (board[currentRow, currentCol].Color!=color)
                    {
                        result.Append(new PiecePosition(currentRow, currentCol));
                    }
                    break;
                }
                currentRow += rowChange;
                currentCol += colChange;
            }
        }
    }
}
