
using Xunit;

namespace SnackCounter.Tests
{
    public class BudgetValidationTests
    {
        [Theory]
        [InlineData(1000, 500, true)]   // Within budget
        [InlineData(1000, 1000, true)] // Exactly budget
        [InlineData(1000, 1200, false)] // Exceeds budget
        [InlineData(0, 100, false)]     // No budget
        public void PurchaseAmount_ShouldBeValidatedAgainstBudget(
            decimal budget,
            decimal purchaseAmount,
            bool expected)
        {
            bool result = purchaseAmount <= budget;

            Assert.Equal(expected, result);
        }
    }
}
