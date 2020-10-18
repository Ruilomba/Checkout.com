namespace Checkout.com.PaymentGateway.Business.DAL.Repositories.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.Common.Mongo.Implementations;
    using Model = Checkout.com.PaymentGateway.Business.DAL.Model;
    using DTO = Checkout.com.PaymentGateway.DTO.Payments;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using Checkout.com.Common.Mongo;
    using Checkout.com.PaymentGateway.Business.Converters;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoDbConnection mongoDbConnection;
        private readonly IMongoCollection<Model.Payment> paymentsCollection;

        public PaymentRepository(IMongoDbConnection mongoDbConnection)
        {
            this.mongoDbConnection = mongoDbConnection ?? throw new ArgumentNullException(nameof(mongoDbConnection));
            this.paymentsCollection = this.mongoDbConnection.GetCollection<Model.Payment>(CollectionNames.Payments);
        }

        public async Task<DTO.Payment> SavePayment(DTO.Payment payment)
        {
            if(payment == null)
            {
                return null;
            }

            var model = payment.ToModel();

            await this.paymentsCollection.InsertOneAsync(model);
            return model.ToDTO();
        }

        public async Task<List<DTO.Payment>> Search(string cardNumber, string merchantId, string customerId)
        {
            var query = this.paymentsCollection.AsQueryable();

            if(cardNumber != null)
            {
                query = query.Where(payment => payment.CardNumber == cardNumber);
            }

            if (merchantId != null)
            {
                query = query.Where(payment => payment.MerchantId == merchantId);
            }

            if (customerId != null)
            {
                query = query.Where(payment => payment.CustomerId == customerId);
            }

            var result =  await query.ToListAsync().ConfigureAwait(false);
            return result?.ToDTO();
        }

        public async Task<DTO.Payment> GetById(Guid id)
        {
            var filter = Builders<Model.Payment>.Filter.Eq(payment => payment.Id, id);

            var result = await this.paymentsCollection.FindAsync(filter);
            return (await result.FirstOrDefaultAsync())?.ToDTO();
        }
    }
}