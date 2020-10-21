namespace Checkout.com.PaymentGateway.Business.Adapters.Implementations
{
    using System.Threading.Tasks;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Utils;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;
    using Checkout.com.PaymentGateway.Business.Converters;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class MasterCardProcessorAdapter : IPaymentProcessorAdapter
    {
        private readonly AcquiringBank.Client.IAcquiringBankClient paymentProcessor;
        private readonly ApplicationSettings applicationSettings;

        public MasterCardProcessorAdapter(
            AcquiringBank.Client.IAcquiringBankClient paymentProcessor,
            ApplicationSettings applicationSettings)
        {
            this.paymentProcessor = paymentProcessor;
            this.applicationSettings = applicationSettings;
        }

        public async Task<PaymentResponse> ProcessPayment(PaymentProcessorPaymentRequest paymentRequest)
        {
            var commissionPaymentValue = paymentRequest.PurchaseValue.GetPercentageValue(paymentRequest.GatewayCommission);
            var merchantPaymentValue = paymentRequest.PurchaseValue - commissionPaymentValue;
            var result = await this.paymentProcessor.ProcessPayment(
                paymentRequest.ToExternalRequest(
                    applicationSettings.GatewayCard,
                    commissionPaymentValue.Value,
                    merchantPaymentValue.Value));

            return result.ToInternalDTO();
        }
    }
}