using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.Domain;
using Xunit;

namespace ChessTests.Domain.ValueObjects
{
    public class PieceNameTests
    {
        [Fact]
        public async Task PieceNameShouldBeCreatedAndMakeTheIdentifierUpperCase()
        {
            var name = new PieceName("king",PieceColor.White);

            Assert.Equal("king",name.Name);
            Assert.Equal("K",name.Identifier);
        }
        [Fact]
        public async Task PieceNameShouldBeCreatedAndMakeTheIdentifierLowerCase()
        {
            var name = new PieceName("king", PieceColor.Black);

            Assert.Equal("king", name.Name);
            Assert.Equal("k", name.Identifier);
        }
        [Theory]
        [InlineData("")]
        [InlineData("s")]
        [InlineData(null)]
        public async Task PieceNameShouldThrowExceptionWhenProvidedWithImproperData(string pieceName) 
            => Assert.Throws<InvalidPieceNameException>(() => new PieceName(pieceName, PieceColor.White));
    }
}
