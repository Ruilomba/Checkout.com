namespace Checkout.com.PaymentGateway.DTO.Card
{
    public class Card
    {
        public ExpirationDate ExpirationDate { get; set; }

        public string CardNumber { get; set; }

        public CardType CardType { get; set; }

        public string CCV { get; set; }
    }
}