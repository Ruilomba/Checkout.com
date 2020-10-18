namespace Checkout.com.AcquiringBank.Client.Implementations
{
    using System.Threading.Tasks;
    using Checkout.com.AcquiringBank.Business.Services;
    using Checkout.com.AcquiringBank.DTO.Payments;

    public class AcquiringBankClient : IAcquiringBankClient
    {
        private readonly IPaymentProcessor paymentProcessor;

        public AcquiringBankClient(IPaymentProcessor paymentProcessor)
        {
            this.paymentProcessor = paymentProcessor;
        }

        public Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            return this.paymentProcessor.ProcessPayment(paymentRequest);
        }
    }
}