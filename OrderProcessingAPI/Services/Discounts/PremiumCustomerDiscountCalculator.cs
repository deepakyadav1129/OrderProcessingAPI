using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Services.Discounts
{
    public class PremiumCustomerDiscountCalculator : IDiscountCalculator
    {
        public bool IsApplicable(CustomerType customerType)
        {
            return customerType == CustomerType.Premium;
        }

        public decimal CalculateDiscount(decimal subTotal)
        {
            // Premium customers receive a 10% discount.
            return Math.Round(subTotal * 0.10m, 2);
        }
    }
}
