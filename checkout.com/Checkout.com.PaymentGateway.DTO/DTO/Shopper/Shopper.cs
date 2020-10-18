using Checkout.com.PaymentGateway.DTO.Card;

namespace Checkout.com.PaymentGateway
{
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
