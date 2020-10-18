namespace Checkout.com.PaymentGateway.DTO.Payments
{
    using System;
    using Newtonsoft.Json;

    public class Payment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string CustomerId { get; set; }

        public string MerchantId { get; set; }

        public decimal Value { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; }
    }
}