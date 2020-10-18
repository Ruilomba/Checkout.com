namespace Checkout.com.PaymentGateway.Business.Adapters
{
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public interface IPaymentProcessor
    {
        Task<PaymentResponse> ProcessPayment(PaymentProcessorPaymentRequest paymentRequest);
    }
}