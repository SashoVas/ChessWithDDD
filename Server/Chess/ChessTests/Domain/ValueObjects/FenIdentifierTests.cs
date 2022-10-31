using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.Domain;
using Xunit;

namespace ChessTests.Domain.ValueObjects
{
    public class FenIdentifierTests
    {
        [Theory]
        [InlineData(DomainConstants.DefaultBoardStartPositionFen)]
        [InlineData("8/8/8/8/8/8/8/8")]
        public async Task FenIdentifierShouldBeCreated(string initialFen)
        {
            var fenIdentifier = FenIdentifier.Create(initialFen);

            Assert.Equal(8, fenIdentifier.Rows.Length);

            for (int i = 0; i < 8; i++)
            {
                int count = 0;
                foreach (var letter in fenIdentifier.Rows[i])
                {
                    if (int.TryParse(letter.ToString(),out int length))
                    {
                        count += length;
                    }
                    else
                    {
                        count++;
                    }
                }
                Assert.Equal(8,count);

            }

            var fenRows = initialFen.Split('/');
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < fenRows[i].Length; j++)
                {
                    Assert.Equal(fenRows[i][j], fenIdentifier.Rows[i][j]);
                }
            }
        }
        [Theory]
        [InlineData("7/7/7/7/7/7/7/7")]
        [InlineData("8/8/8/8/8/8/8/7")]
        [InlineData(null)]
        [InlineData("")]
        public async Task FenIdentifierShouldThrowExceptionWhenProvidedWithImproperData(string initialFen) 
            => Assert.Throws<InvalidLengthForAFenException>(() => FenIdentifier.Create(initialFen));
    }
}
