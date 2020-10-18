namespace Checkout.com.PaymentGateway.Business.Adapters.Implementations
{
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class VisaProcessorAdapter : IPaymentProcessorAdapter
    {
        public Task<PaymentResponse> ProcessPayment(PaymentProcessorPaymentRequest paymentRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}