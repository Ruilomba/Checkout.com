namespace checkout.com.Tests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using Checkout.com.Common.Mongo;
    using Checkout.com.Common.Mongo.Implementations;
    using MongoDB.Driver;

    public class BaseTests
    {
        protected readonly IMongoDbConnection mongoDBConnection;
        protected readonly Fixture Fixture = new Fixture();
        protected const string connectionString = "mongodb+srv://checkout_payment_gateway:checkoutUser@cluster0.bwyrg.mongodb.net/payment_gateway_tests?retryWrites=true&w=majority";

        public BaseTests()
        {
            this.mongoDBConnection = new MongoDbConnection(connectionString);
            this.mongoDBConnection.Connect();
        }

        protected async Task DeleteAll()
        {
            var filter = Builders<Checkout.com.PaymentGateway.Business.DAL.Model.Payment>.Filter.Empty;
            await this.mongoDBConnection.GetCollection<Checkout.com.PaymentGateway.Business.DAL.Model.Payment>(CollectionNames.Payments)
                .DeleteManyAsync(filter);
        }

        protected async Task InsertPayment(Checkout.com.PaymentGateway.Business.DAL.Model.Payment paymentToAdd)
        {
            await this.mongoDBConnection.GetCollection<Checkout.com.PaymentGateway.Business.DAL.Model.Payment>(CollectionNames.Payments)
                .InsertOneAsync(paymentToAdd);
        }
    }
}