namespace Checkout.com.PaymentGateway
{
    using Checkout.com.PaymentGateway.DTO.Card;

    public class Shopper
    {
        public User User { get; set; }

        public Card Card { get; set; }

        public CardType GetCardType()
        {
            return this.Card.CardType;
        }
    }
}