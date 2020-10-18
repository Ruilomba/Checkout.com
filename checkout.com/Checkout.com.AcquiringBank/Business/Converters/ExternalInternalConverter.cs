namespace Checkout.com.AcquiringBank.Business.Converters
{
    using Internal = DTO;
    using External = _3rdPartyBank.DTO;
    using System;

    public static class ExternalInternalConverter
    {
        public static External.Payments.PaymentRequest ToExternal(this Internal.Payments.PaymentRequest paymentRequest)
        {
            return new External.Payments.PaymentRequest
            {
                CommissionPayment = paymentRequest.CommissionPayment.ToExternal(),
                MerchantPayment = paymentRequest.MerchantPayment.ToExternal()
            };
        }

        public static External.Payments.Payment ToExternal(this Internal.Payments.Payment payment)
        {
            return new External.Payments.Payment
            {
                CardToDeposit = payment.CardToDeposit.ToExternal(),
                CardToWithraw = payment.CardToWithraw.ToExternal(),
                CurrencyCode = payment.CurrencyCode,
                Value = payment.Value
            };
        }

        public static External.Card.Card ToExternal(this Internal.Card.Card card)
        {
            return new External.Card.Card
            {
                CardNumber = card.CardNumber,
                CCV = card.CCV,
                ExpirationDate = new External.Card.ExpirationDate
                {
                    Month = card.ExpirationDate.Month,
                    Year = card.ExpirationDate.Year
                }
            };
        }

        public static Internal.Payments.PaymentResponse ToInternal(this External.Payments.PaymentResponse paymentResponse)
        {
            return new Internal.Payments.PaymentResponse
            {
                PaymentStatus = paymentResponse.PaymentStatus.ToInternal()
            };
        }

        public static Internal.Payments.PaymentStatus ToInternal(this External.Payments.PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case External.Payments.PaymentStatus.Accepted:
                    return Internal.Payments.PaymentStatus.Accepted;
                case External.Payments.PaymentStatus.Declined:
                    return Internal.Payments.PaymentStatus.Declined;
                default:
                    throw new NotSupportedException("Payment status type not recognized");
            }
        }
    }
}