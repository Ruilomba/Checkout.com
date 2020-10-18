namespace Checkout.com.PaymentGateway.Business.Services
{
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest);
    }
}