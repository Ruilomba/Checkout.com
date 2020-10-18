namespace Checkout.com.AcquiringBank.Client
{
    using System.Threading.Tasks;
    using Checkout.com.AcquiringBank.DTO.Payments;

    public interface IAcquiringBankClient
    {
        Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest);
    }
}