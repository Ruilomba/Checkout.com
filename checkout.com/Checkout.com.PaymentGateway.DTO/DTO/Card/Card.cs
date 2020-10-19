namespace Checkout.com.PaymentGateway.DTO.Card
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Card
    {
        public ExpirationDate ExpirationDate { get; set; }

        public string CardNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CardType CardType { get; set; }

        public string CCV { get; set; }
    }
}