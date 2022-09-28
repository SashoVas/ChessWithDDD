using Domain.Entities;
using Domain.Policies;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IPieceFactory pieceFactory;

        public BoardFactory(IPieceFactory pieceFactory) 
            => this.pieceFactory = pieceFactory;

        public Board CreateCustomStandard(FenIdentifier fen)
        {
            var policy = new CustomStandardBoardPolicy();
            var pieces = policy.GenerateItems(pieceFactory, fen);
            var board = new Board(Guid.NewGuid(),pieces.ToList());
            return board;
        }

        public Board CreateStandardBoard()
        {
            var policy = new StandardBoardPolicy();
            var pieces = policy.GenerateItems(pieceFactory, null);
            var board = new Board(Guid.NewGuid(),pieces.ToList());
            return board;
        }
    }
}
