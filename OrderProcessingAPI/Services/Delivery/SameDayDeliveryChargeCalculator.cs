namespace OrderProcessingAPI.Services.Delivery
{
    public class SameDayDeliveryChargeCalculator : IDeliveryChargeCalculator
    {
        public bool IsApplicable(string deliveryMethod)
        {
            return deliveryMethod.Equals("SameDay", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateDeliveryCharge(decimal orderAmount)
        {
            return 200;
        }
    }
}
