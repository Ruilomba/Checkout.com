namespace Checkout.com.Common.Mongo.Implementations
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Driver;

    public sealed class MongoDbConnection : IMongoDbConnection
    {
        private readonly string connectionString;

        private MongoClient mongoClient;

        private MongoUrl mongoUrl;

        public MongoDbConnection(string connectionString)
        {
            this.connectionString = connectionString;
            this.Connect();
        }

        public void Connect()
        {
            this.mongoClient = new MongoClient(connectionString);
            var dbList = mongoClient.ListDatabases().ToList();

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