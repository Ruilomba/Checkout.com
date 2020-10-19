namespace Checkout.com.PaymentGateway.DTO.Payments
{
    public class PaymentRequest
    {
        public Merchant Merchant { get; set; }

        public Shopper Shopper { get; set; }

        public Money PurchaseValue { get; set; }
    }
}