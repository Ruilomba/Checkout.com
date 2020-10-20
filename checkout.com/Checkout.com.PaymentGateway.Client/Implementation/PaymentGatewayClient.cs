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
        public readonly string controllerAddress = "api/payments";

        public PaymentGatewayClient(string baseAddress) : base(baseAddress)
        {
        }

        public async Task<Payment> Create(PaymentRequest paymentRequest)
        {
            var url = this.GenerateUri($"{this.httpClient.BaseAddress}{this.controllerAddress}");
            return await this.PostAsync<Payment, PaymentRequest>(url, paymentRequest);
        }

        public async Task<Payment> Get(Guid id)
        {
            var uri = this.GenerateUri(
                $"{this.httpClient.BaseAddress}{this.controllerAddress}/{id}");
            return await this.GetAsync<Payment>(uri);
        }

        public async Task<List<Payment>> Search(string cardNumber, string merchantId, string customerId)
        {
            var queryStringParams = new Dictionary<string, string>();

            if(cardNumber != null)
            {
                queryStringParams.Add("cardNumber", cardNumber);
            }

            if(merchantId != null)
            {
                queryStringParams.Add("merchantId", merchantId);
            }

            if (customerId != null)
            {
                queryStringParams.Add("customerId", customerId);
            }

            var uri = this.GenerateUri(
                $"{this.httpClient.BaseAddress}{this.controllerAddress}/search",
                queryStringParams);

            return await this.GetAsync<List<Payment>>(uri);
        }

        public Uri GenerateUri(string resourceUrl, Dictionary<string, string> queryStringParams = null)
        {
            if (queryStringParams?.Any() == true)
            {
                resourceUrl += "?" + string.Join("&", queryStringParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            }

            return new Uri(resourceUrl);
        }
    }
}