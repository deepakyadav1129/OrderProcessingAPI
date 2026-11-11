namespace OrderProcessingAPI.Services.Delivery
{
    public interface IDeliveryChargeCalculator
    {
        bool IsApplicable(string deliveryMethod);
        decimal CalculateDeliveryCharge(decimal orderAmount);
    }
}
