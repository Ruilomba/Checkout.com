namespace Checkout.com.PaymentGateway
{
    using System.Collections.Generic;
    using Checkout.com.PaymentGateway.DTO.Card;

    public class Shopper
    {
        public User User { get; set; }

        public Card Card { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Shopper shopper &&
                   EqualityComparer<User>.Default.Equals(User, shopper.User) &&
                   EqualityComparer<Card>.Default.Equals(Card, shopper.Card);
        }

        public CardType GetCardType()
        {
            return this.Card.CardType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1804280673;
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(User);
            hashCode = hashCode * -1521134295 + EqualityComparer<Card>.Default.GetHashCode(Card);
            return hashCode;
        }
    }
}