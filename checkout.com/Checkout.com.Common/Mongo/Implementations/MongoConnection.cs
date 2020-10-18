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
        }

        internal static IMongoDbConnection BuildFrom(string connectionString)
        {
            return new MongoDbConnection(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));
        }

        public void Connect()
        {
            void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            this.mongoUrl = new MongoUrl(this.connectionString);

            var settings = MongoClientSettings.FromUrl(this.mongoUrl);
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
            settings.ClusterConfigurator = builder => builder.ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));

            this.mongoClient = new MongoClient(settings);
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