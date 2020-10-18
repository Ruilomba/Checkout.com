namespace Checkout.com.Common.Configuration
{
    using Checkout.com.Common.Configuration.Entities;

    public class ApplicationSettings
    {
        public GatewayCard GatewayCard { get; set; }

        public string Secret { get; set; }
    }
}