namespace Checkout.com.PaymentGateway.Client.Implementation
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public abstract class Client
    {
        protected readonly HttpClient httpClient;

        public Client(string baseAddress)
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<TResult> PostAsync<TResult, TRequest>(Uri uri, TRequest request)
        {
            var stringPayload = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(uri, httpContent);
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(responseContent);
            }

            return default;
        }

        public async Task<TResult> GetAsync<TResult>(Uri uri)
        {
            var httpResponse = await this.httpClient.GetAsync(uri);
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(responseContent);
            }

            return default;
        }
    }
}