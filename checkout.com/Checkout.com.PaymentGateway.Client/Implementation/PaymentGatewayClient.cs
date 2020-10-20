namespace Checkout.com.PaymentGateway.Client.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class PaymentGatewayClient : Client, IPaymentGatewayClient
    {
        private readonly HttpClient httpClient;

        public readonly string controllerAddress = "payments";

        public PaymentGatewayClient(string baseAddress) : base(baseAddress)
        {
        }

        public async Task<PaymentResponse> Create(PaymentRequest paymentRequest)
        {
            var url = this.GenerateUri($"{this.httpClient.BaseAddress}/{this.controllerAddress}");
            return await this.PostAsync<PaymentResponse, PaymentRequest>(url, paymentRequest);
        }

        public async Task<Payment> Get(Guid id)
        {
            var uri = this.GenerateUri(
                $"{this.httpClient.BaseAddress}/{this.controllerAddress}",
                new Dictionary<string, string> 
                {
                    {"id", id.ToString()}
                });
            return await this.GetAsync<Payment>(uri);
        }

        public async Task<List<Payment>> Search(string cardNumber, string merchantId, string customerId)
        {
            var uri = this.GenerateUri(
                $"{this.httpClient.BaseAddress}/{this.controllerAddress}",
                queryStringParams: new Dictionary<string, string> 
                {
                    {"cardNumber", cardNumber },
                    {"merchantId", merchantId },
                    {"customerId", customerId }
                });

            return await this.GetAsync<List<Payment>>(uri);
        }

        public Uri GenerateUri(string resourceUrl, Dictionary<string, string> addressParams = null, Dictionary<string, string> queryStringParams = null)
        {
            if (addressParams?.Keys?.Any() == true)
            {
                foreach (var key in addressParams.Keys)
                {
                    resourceUrl = resourceUrl.Replace("{" + key + "}", addressParams[key]);
                }
            }

            if (queryStringParams?.Any() == true)
            {
                resourceUrl += "?" + string.Join("&", queryStringParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            }

            return new Uri(resourceUrl);
        }
    }
}