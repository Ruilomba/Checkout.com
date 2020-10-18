namespace Checkout.com.Common.Configuration.Entities
{
    public class GatewayCard
    {
        public string CardNumber { get; set; }

        public string CCV { get; set; }

        public ExpirationDate ExpirationDate { get; set; }
    }
}
