namespace Checkout.com.AcquiringBank.Business.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using _3rdPartyBank;
    using Checkout.com.AcquiringBank.Business.Converters;
    using Checkout.com.AcquiringBank.DTO.Card;
    using Checkout.com.AcquiringBank.DTO.Payments;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Utils;

    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly ApplicationSettings applicationSettings;
        private readonly I3rdPartyBankAPI bankClient;

        public PaymentProcessor(ApplicationSettings applicationSettings, I3rdPartyBankAPI bankClient)
        {
            this.applicationSettings = applicationSettings;
            this.bankClient = bankClient;
        }
        public async Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            this.ValidateRequest(paymentRequest);
            this.DecriptData(paymentRequest);

            var result = await this.bankClient.Process(paymentRequest.ToExternal());
            return result?.ToInternal();
        }

        private void ValidateRequest(PaymentRequest paymentRequest)
        {
            if (paymentRequest.CommissionPayment == null)
            {
                throw new NotSupportedException("CommissionPayment cannot be null");
            }

            if (paymentRequest.MerchantPayment == null)
            {
                throw new NotSupportedException("MerchantPayment cannot be null");
            }

            if (paymentRequest.CommissionPayment.Value <= 0)
            {
                throw new NotSupportedException("Commission payment value cannot be 0 or negative");
            }

            if (paymentRequest.MerchantPayment.Value <= 0)
            {
                throw new NotSupportedException("Commission payment value cannot be 0 or negative");
            }

            this.ValidateCard(paymentRequest.MerchantPayment.CardToDeposit);
            this.ValidateCard(paymentRequest.MerchantPayment.CardToWithraw);
            this.ValidateCard(paymentRequest.CommissionPayment.CardToDeposit);
            this.ValidateCard(paymentRequest.CommissionPayment.CardToWithraw);
        }

        private void DecriptData(PaymentRequest paymentRequest)
        {
            this.DecryptCard(paymentRequest.CommissionPayment.CardToDeposit);
            this.DecryptCard(paymentRequest.CommissionPayment.CardToWithraw);
            this.DecryptCard(paymentRequest.MerchantPayment.CardToWithraw);
            this.DecryptCard(paymentRequest.MerchantPayment.CardToDeposit);
        }

        private void DecryptCard(Card card)
        {
            card.CardNumber = card.CardNumber.DecryptString(applicationSettings.Secret);
            card.CCV = card.CardNumber.DecryptString(applicationSettings.Secret);
        }

        private void ValidateCard(Card card)
        {
            if (card == null)
            {
                throw new ArgumentException("Merchant Card cannot be null");
            }

            if (card.CardNumber == null)
            {
                throw new ArgumentException("Merchant Card number cannot be null");
            }

            if (!int.TryParse(card.CardNumber.Trim(), out _))
            {
                throw new ArgumentException("Merchant Card number is not valid");
            }

            if (card.CCV == null)
            {
                throw new ArgumentException("Merchant Card CCV cannot be null");
            }

            if (!int.TryParse(card.CCV, out _))
            {
                throw new ArgumentException("Merchant Card CCV is not valid");
            }

            if (card.ExpirationDate == null)
            {
                throw new ArgumentException("Merchant Card Expiration Date cannot be null");
            }

            var dateTimeNow = DateTime.UtcNow;

            if (card.ExpirationDate.Year > dateTimeNow.Year
                || (card.ExpirationDate.Year == dateTimeNow.Year
                && card.ExpirationDate.Month > dateTimeNow.Month))
            {
                throw new ArgumentException("Merchant Card is expired");
            }
        }
    }
}