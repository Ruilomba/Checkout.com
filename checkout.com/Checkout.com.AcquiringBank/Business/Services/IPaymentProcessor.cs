namespace Checkout.com.AcquiringBank.Business.Services
{
    using System.Threading.Tasks;
    using Checkout.com.AcquiringBank.DTO.Payments;

    public interface IPaymentProcessor
    {
        Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest);
    }
}