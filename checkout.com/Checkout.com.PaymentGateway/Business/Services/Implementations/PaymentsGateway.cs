namespace Presentation.Api.Business.Gateway.Implementations
{
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.Business.Services;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class PaymentsGateway
    {
        private readonly IPaymentService paymentService;

        public PaymentsGateway(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            return this.paymentService.ProcessPayment(paymentRequest);
        }
    }
}