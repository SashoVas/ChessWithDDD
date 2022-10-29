using Domain.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace ChessTests.Domain.ValueObjects
{
    public class PiecePositionTests
    {
        [Fact]
        public async Task ThePositionShouldInitialize()
        {
            var position = new PiecePosition(1,1);

            Assert.NotNull(position);
            Assert.Equal(1, position.Row);
            Assert.Equal(1, position.Col);
        }
        [Theory]
        [InlineData(10, 1)]
        [InlineData(1, 10)]
        public async Task ThePositionShouldThrowExceptionWhenInitializedWithImproperData(int row, int col) 
            => Assert.Throws<InvalidPositionParametersException>(() => new PiecePosition(row, col));
    }
}
