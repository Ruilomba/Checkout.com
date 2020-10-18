namespace Checkout.com.PaymentGateway
{
    using Checkout.com.PaymentGateway.DTO.Card;

    public class Merchant
    {
        public string Id { get; set; }

        public string CurrencyCode { get; set; }

        public Card Card { get; set; }
    }
}