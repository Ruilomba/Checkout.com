namespace Checkout.com.PaymentGateway.DTO.Payments
{
    using Checkout.com.Common;
    using Checkout.com.PaymentGateway.DTO.Card;

    public class PaymentRequest
    {
        public Merchant Merchant { get; set; }

        public Shopper Shopper { get; set; }

        public Money PurchaseValue { get; set; }

        public CardType CardType { get; set; }
    }
}