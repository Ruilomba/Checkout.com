namespace Checkout.com.AcquiringBank.Business.Services.Implementations
{
    using System.Threading.Tasks;
    using Checkout.com.AcquiringBank.DTO.Card;
    using Checkout.com.AcquiringBank.DTO.Payments;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Utils;

    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly ApplicationSettings applicationSettings;

        public PaymentProcessor(ApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }
        public Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            this.DecryptPassword(paymentRequest);
            //TODO

            return null;
        }

        private void DecryptPassword(PaymentRequest paymentRequest)
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
    }
}