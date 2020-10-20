namespace Checkout.com.PaymentGateway.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public interface IPaymentService
    {
        Task<Payment> ProcessPayment(PaymentRequest paymentRequest);

        Task<Payment> GetPaymentById(Guid id);

        Task<List<Payment>> SearchPayments(string cardNumber, string merchantId, string customerId);
    }
}