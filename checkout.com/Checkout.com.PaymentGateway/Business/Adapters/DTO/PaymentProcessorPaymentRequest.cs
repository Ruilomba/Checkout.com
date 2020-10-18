namespace Checkout.com.PaymentGateway.Business.Adapters.DTO
{
    using Checkout.com.Common;

    public class PaymentProcessorPaymentRequest
    {
        public Merchant Merchant { get; set; }

        public Shopper Shopper { get; set; }

        public Money PurchaseValue { get; set; }

        public decimal GatewayCommission { get; set; }
    }
}