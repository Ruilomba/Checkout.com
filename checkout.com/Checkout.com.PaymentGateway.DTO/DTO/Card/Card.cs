using System.Collections.Generic;

namespace Checkout.com.PaymentGateway.DTO.Card
{
    public class Card
    {
        public ExpirationDate ExpirationDate { get; set; }

        public string CardNumber { get; set; }

        public CardType CardType { get; set; }

        public string CCV { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Card card &&
                   EqualityComparer<ExpirationDate>.Default.Equals(ExpirationDate, card.ExpirationDate) &&
                   CardNumber == card.CardNumber &&
                   CardType == card.CardType &&
                   CCV == card.CCV;
        }

        public override int GetHashCode()
        {
            int hashCode = -1121411640;
            hashCode = hashCode * -1521134295 + EqualityComparer<ExpirationDate>.Default.GetHashCode(ExpirationDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardNumber);
            hashCode = hashCode * -1521134295 + CardType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CCV);
            return hashCode;
        }
    }
}