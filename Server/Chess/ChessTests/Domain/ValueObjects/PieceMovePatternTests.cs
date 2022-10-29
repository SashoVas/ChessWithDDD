using Domain.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace ChessTests.Domain.ValueObjects
{
    public class PieceMovePatternTests
    {
        [Fact]
        public async Task PieceMovePatterShouldBeCreated()
        {
            var pieceMovePatter = new PieceMovePattern(true,true,1,1,PieceColor.White);

            Assert.Equal(1, pieceMovePatter.ColChange);
            Assert.Equal(1, pieceMovePatter.RowChange);
            Assert.True(pieceMovePatter.IsRepeatable);
            Assert.True(pieceMovePatter.SwapDirections);
        }
        [Fact]
        public async Task PieceMovePatterShouldMakeColChangeAndRowChangeNegative()
        {
            var pieceMovePatter = new PieceMovePattern(true, true, 1, 1, PieceColor.Black);

            Assert.Equal(-1, pieceMovePatter.ColChange);
            Assert.Equal(-1, pieceMovePatter.RowChange);
            Assert.True(pieceMovePatter.IsRepeatable);
            Assert.True(pieceMovePatter.SwapDirections);
        }
        [Theory]
        [InlineData(10, 1)]
        [InlineData(1, 10)]
        [InlineData(0, 0)]
        public async Task PieceMovePatternShouldThrowExceptionWhenInititalizedWithImproperData(int rowChange, int colChange) 
            => Assert.Throws<InvalidValuesForAMoveException>(() => new PieceMovePattern(true, true, rowChange, colChange, PieceColor.White));

    }
}
