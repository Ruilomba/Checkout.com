namespace Checkout.com.AcquiringBank.DTO.Payments
{
    using Checkout.com.AcquiringBank.DTO.Card;

    public class Payment
    {
        public Card CardToDeposit { get; set; }

        public Card CardToWithraw { get; set; }

        public decimal Value { get; set; }

        public string CurrencyCode { get; set; }
    }
}