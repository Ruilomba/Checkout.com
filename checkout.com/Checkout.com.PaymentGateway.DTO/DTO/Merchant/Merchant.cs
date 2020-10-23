namespace Checkout.com.PaymentGateway
{
    using System.Collections.Generic;
    using Checkout.com.PaymentGateway.DTO.Card;

    public class Merchant
    {
        public string Id { get; set; }

        public string CurrencyCode { get; set; }

        public Card Card { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Merchant merchant &&
                   Id == merchant.Id &&
                   CurrencyCode == merchant.CurrencyCode &&
                   EqualityComparer<Card>.Default.Equals(Card, merchant.Card);
        }

        public override int GetHashCode()
        {
            int hashCode = 1516156546;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrencyCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<Card>.Default.GetHashCode(Card);
            return hashCode;
        }
    }
}