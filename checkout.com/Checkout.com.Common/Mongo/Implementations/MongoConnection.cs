namespace Checkout.com.Common.Mongo.Implementations
{
    using MongoDB.Driver;

    public sealed class MongoDbConnection : IMongoDbConnection
    {
        private MongoClient mongoClient;

        private MongoUrl mongoUrl;

        public MongoDbConnection(string connectionString)
        {
            this.mongoUrl = new MongoUrl(connectionString);
            this.Connect();
        }

        public void Connect()
        {
            this.mongoClient = new MongoClient(this.mongoUrl);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var database = this.GetDatabase();
            return database.GetCollection<T>(collectionName);
        }

        private IMongoDatabase GetDatabase() => this.mongoClient.GetDatabase(this.mongoUrl.DatabaseName);

        public void Shutdown()
        {
            this.mongoUrl = null;
            this.mongoClient = null;
        }
    }
}