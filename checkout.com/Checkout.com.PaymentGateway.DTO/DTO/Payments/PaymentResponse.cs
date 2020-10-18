namespace Checkout.com.PaymentGateway.DTO.Payments
{
    using System;

    public class PaymentResponse
    {
        public PaymentStatus PaymentStatus { get; set; }

        public Guid PaymentId { get; set; }
    }
}