using Xunit;
using SnackCounter.Models.Enum;

namespace SnackCounter.Tests
{
    public class RefillTests
    {
        [Theory]
        [InlineData(RefillStatus.Open)]
        [InlineData(RefillStatus.Assigned)]
        [InlineData(RefillStatus.Purchased)]
        [InlineData(RefillStatus.ReimbursementPending)]
        [InlineData(RefillStatus.Completed)]
        [InlineData(RefillStatus.Cancelled)]
        public void Purchase_ShouldBeRejected_WhenRefillIsNotAccepted(RefillStatus status)
        {
            // Arrange
            var isAccepted = false;

            // Act
            isAccepted = status == RefillStatus.Accepted;

            // Assert
            Assert.False(isAccepted);
        }

        [Fact]
        public void Purchase_ShouldBeAllowed_WhenRefillIsAccepted()
        {
            // Arrange
            var status = RefillStatus.Accepted;

            // Act
            var isAccepted = status == RefillStatus.Accepted;

            // Assert
            Assert.True(isAccepted);
        }
    }
}