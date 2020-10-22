namespace Checkout.com.Tests.End2EndTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using checkout.com.Tests.Utils;
    using Checkout.com.Tests.Input;
    using Checkout.com.PaymentGateway.Client;
    using Checkout.com.PaymentGateway.Client.Implementation;
    using Checkout.com.PaymentGateway.DTO.Payments;
    using Xunit;
    using checkout.com.Tests;

    public class End2EndTests : BaseTests
    {
        private readonly IPaymentGatewayClient paymentGatewayClient;
        private const string cardNumber = "1234";
        private const string cardNumberEncrypted = "H+TW00KCel3dWHyqA+mlAg==";

        public End2EndTests() : base()
        {
            this.paymentGatewayClient = new PaymentGatewayClient("http://localhost:5000");
        }

        [Theory]
        [JsonFileData("End2EndTests/Data/process_payment.json")]
        public async Task ProcessPayment_ShouldProcess(dynamic input, dynamic expectedResponse)
        {
            await this.DeleteAll();
            PaymentRequest request = Utils.GetData<PaymentRequest>(input);
            var result = await this.paymentGatewayClient.Create(request);
            var expectedPayment = Utils.GetData<Payment>(expectedResponse.Payment);
            Assert.Equal(expectedPayment.Status, result.Status);
            Assert.Equal(expectedPayment.MerchantId, result.MerchantId);
            Assert.Equal(expectedPayment.Value, result.Value);
            Assert.Equal(expectedPayment.CardNumber, result.CardNumber);
            Assert.Equal(expectedPayment.CurrencyCode, result.CurrencyCode);
            Assert.Equal(expectedPayment.CustomerId, result.CustomerId);
            await DeleteAll();
        }

        [Fact]
        public async Task SearchPayments_ShouldRetriveData()
        {
            await this.DeleteAll();
            var paymentToAdd = this.Fixture.Build<PaymentGateway.Business.DAL.Model.Payment>()
                .With(x => x.CardNumber, "H+TW00KCel3dWHyqA+mlAg==")
                .Create();
            await this.InsertPayment(paymentToAdd);

            var result = await this.paymentGatewayClient.Search(
                cardNumber,
                Utils.GetData<string>(paymentToAdd.MerchantId),
                Utils.GetData<string>(paymentToAdd.CustomerId));

            Assert.NotEmpty(result);
            Assert.Contains(
                result,
                payment => payment.CustomerId == paymentToAdd.CustomerId 
                           && payment.CardNumber == cardNumber
                           && payment.MerchantId == paymentToAdd.MerchantId);

            await DeleteAll();
        }

        [Fact]
        public async Task GetPaymentById_ShouldRetriveData()
        {
            await this.DeleteAll();
            var paymentToAdd = this.Fixture.Build<PaymentGateway.Business.DAL.Model.Payment>()
                .With(x => x.CardNumber, cardNumberEncrypted)
                .Create();

            await this.InsertPayment(paymentToAdd);

            var result = await this.paymentGatewayClient.Get(paymentToAdd.Id);

            Assert.NotNull(result);
            await DeleteAll();
        }
    }
}