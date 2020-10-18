namespace Checkout.com.Common.Configuration
{
    using System.Collections.Generic;
    using Checkout.com.Common.Configuration.Entities;

    public class ApplicationSettings
    {
        public GatewayCard GatewayCard { get; set; }

        public Dictionary<string, string> ConnectionStrings { get; set; }

        public string Secret { get; set; }
    }
}