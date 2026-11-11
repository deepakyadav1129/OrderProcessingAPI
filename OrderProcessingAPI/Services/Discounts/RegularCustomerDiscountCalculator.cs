using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Services.Discounts
{
    public class RegularCustomerDiscountCalculator : IDiscountCalculator
    {
        public bool IsApplicable(CustomerType customerType)
        {
            return customerType == CustomerType.Regular;
        }

        public decimal CalculateDiscount(decimal subTotal)
        {
            // Regular customers receive no discount.
            return 0;
        }
    }
}
