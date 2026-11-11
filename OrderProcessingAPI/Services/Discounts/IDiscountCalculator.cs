using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Services.Discounts
{
    public interface IDiscountCalculator
    {
        bool IsApplicable(CustomerType customerType);
        decimal CalculateDiscount(decimal subTotal);
    }
}
