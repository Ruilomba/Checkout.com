
namespace Checkout.com.Common.Mongo.Implementations
{
    using MongoDB.Driver;

    public interface IMongoDbConnection
    {
        void Connect();

        void Shutdown();

        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
