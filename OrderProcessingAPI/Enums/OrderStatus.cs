namespace OrderProcessingAPI.Enums
{
    public enum OrderStatus
    {
        // Order created, payment processing not completed
        Pending = 1,

        // Card/UPI payment succeeded OR
        // COD order has been accepted.
        Confirmed = 2,

        // Online payment failed
        Failed = 3
    }
}
