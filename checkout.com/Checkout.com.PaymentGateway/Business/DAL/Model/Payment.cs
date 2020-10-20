using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Checkout.com.PaymentGateway.Business.DAL.Model
{
    public class Payment
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string CustomerId { get; set; }

        public string MerchantId { get; set; }

        public decimal Value { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime PaymentDate{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]  
        [BsonRepresentation(BsonType.String)]         
        public PaymentStatus Status { get; set; }

    }
}
