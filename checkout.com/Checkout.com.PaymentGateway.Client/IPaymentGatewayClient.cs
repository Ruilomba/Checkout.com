namespace Checkout.com.PaymentGateway.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public interface IPaymentGatewayClient
    {
        Task<List<Payment>> Search(string cardNumber, string merchantId, string customerId);

        Task<Payment> Get(Guid paymentId);

        Task<Payment> Create(PaymentRequest paymentRequest);
    }
}
