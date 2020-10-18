namespace Checkout.com.PaymentGateway.Business.DAL.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public interface IPaymentRepository
    {
        Task<Payment> SavePayment(Payment payment);

        Task<List<Payment>> Search(string cardNumber, string merchantId, string customerId);

        Task<Payment> GetById(Guid id);
    }
}