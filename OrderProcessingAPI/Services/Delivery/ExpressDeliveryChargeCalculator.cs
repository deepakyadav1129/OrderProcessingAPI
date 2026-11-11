namespace OrderProcessingAPI.Services.Delivery
{
    public class ExpressDeliveryChargeCalculator : IDeliveryChargeCalculator
    {
        public bool IsApplicable(string deliveryMethod)
        {
            return deliveryMethod.Equals("Express", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateDeliveryCharge(decimal orderAmount)
        {
            return 100;
        }
    }
}
