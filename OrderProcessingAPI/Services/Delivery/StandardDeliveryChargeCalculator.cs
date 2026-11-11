namespace OrderProcessingAPI.Services.Delivery
{
    public class StandardDeliveryChargeCalculator : IDeliveryChargeCalculator
    {
        public bool IsApplicable(string deliveryMethod)
        {
            return deliveryMethod.Equals("Standard", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateDeliveryCharge(decimal orderAmount)
        {
            return 50;
        }
    }
}