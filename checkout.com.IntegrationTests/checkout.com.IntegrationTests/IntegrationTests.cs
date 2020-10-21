namespace Checkout.com.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using checkout.com.IntegrationTests.Utils;
    using Checkout.com.Common.Mongo;
    using Checkout.com.Common.Mongo.Implementations;
    using Checkout.com.IntegrationTests.Input;
    using Checkout.com.PaymentGateway.Client;
    using Checkout.com.PaymentGateway.Client.Implementation;
    using Checkout.com.PaymentGateway.DTO.Payments;
    using MongoDB.Driver;
    using Xunit;

    public class IntegrationTests
    {
        private readonly IPaymentGatewayClient paymentGatewayClient;
        private readonly IMongoDbConnection mongoConnection;
        private const string cardNumber = "1234";
        private const string cardNumberEncrypted = "H+TW00KCel3dWHyqA+mlAg==";
        private readonly Fixture Fixture = new Fixture();
        private const string connectionString = "mongodb+srv://checkout_payment_gateway:checkoutUser@cluster0.bwyrg.mongodb.net/payment_gateway_tests?retryWrites=true&w=majority";

        public IntegrationTests()
        {
            this.mongoConnection = new MongoDbConnection(connectionString);
            this.mongoConnection.Connect();
            this.paymentGatewayClient = new PaymentGatewayClient("https://localhost:44329");
        }

        [Theory]
        [JsonFileData("Input/Files/process_payment.json")]
        public async Task ProcessPayment_ShouldProcess(dynamic input, dynamic expectedResponse)
        {
            PaymentRequest request = Utils.GetData<PaymentRequest>(input);
            var result = await this.paymentGatewayClient.Create(request);
            var expectedPayment = Utils.GetData<Payment>(expectedResponse.Payment);
            Assert.Equal(expectedPayment.Status, result.Status);
            Assert.Equal(expectedPayment.MerchantId, result.MerchantId);
            Assert.Equal(expectedPayment.Value, result.Value);
            Assert.Equal(expectedPayment.CardNumber, result.CardNumber);
            Assert.Equal(expectedPayment.CurrencyCode, result.CurrencyCode);
            Assert.Equal(expectedPayment.CustomerId, result.CustomerId);
            await ClearPayment(result.Id);
        }

        [Fact]
        public async Task SearchPayments_ShouldRetriveData()
        {
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

            await ClearPayment(paymentToAdd.Id);
        }

        [Fact]
        public async Task GetPaymentById_ShouldRetriveData()
        {
            var paymentToAdd = this.Fixture.Build<PaymentGateway.Business.DAL.Model.Payment>()
                .With(x => x.CardNumber, cardNumberEncrypted)
                .Create();

            await this.InsertPayment(paymentToAdd);

            var result = await this.paymentGatewayClient.Get(paymentToAdd.Id);

            Assert.NotNull(result);
            await ClearPayment(paymentToAdd.Id);
        }

        private async Task ClearPayment(Guid id)
        {
            var filter = Builders<PaymentGateway.Business.DAL.Model.Payment>.Filter.Eq(payment => payment.Id, id);
            await this.mongoConnection.GetCollection<PaymentGateway.Business.DAL.Model.Payment>(CollectionNames.Payments)
                .FindOneAndDeleteAsync(filter);
        }

        private async Task InsertPayment(PaymentGateway.Business.DAL.Model.Payment paymentToAdd)
        {
            await this.mongoConnection.GetCollection<PaymentGateway.Business.DAL.Model.Payment>(CollectionNames.Payments)
                .InsertOneAsync(paymentToAdd);
        }
    }
}