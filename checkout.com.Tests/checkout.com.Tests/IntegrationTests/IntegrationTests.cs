namespace checkout.com.Tests.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories.Implementations;
    using Checkout.com.PaymentGateway.DTO.Payments;
    using Xunit;

    public class IntegrationTests : BaseTests
    {
        private readonly IPaymentRepository paymentRepository;

        public IntegrationTests() : base()
        {
            this.paymentRepository = new PaymentRepository(this.mongoDBConnection);
        }

        [Fact]
        public async Task ShouldSaveAndFetchPaymentById()
        {
            await this.DeleteAll();
            var payment = this.Fixture.Build<Payment>()
                .With(x => x.PaymentDate, DateTime.UtcNow)
                .Create();

            var savedPayment = await this.paymentRepository.SavePayment(payment);

            var result = await this.paymentRepository.GetById(savedPayment.Id);

            Assert.NotNull(result);
            this.AssertEqual(savedPayment, result);
            await this.DeleteAll();
        }


        [Fact]
        public async Task ShouldSaveAndFetchPaymentUsingSearch()
        {
            await this.DeleteAll();
            var payment = this.Fixture.Build<Payment>()
                .With(x => x.PaymentDate, DateTime.UtcNow)
                .Create();

            var savedPayment = await this.paymentRepository.SavePayment(payment);

            var result = await this.paymentRepository.Search(savedPayment.CardNumber, savedPayment.MerchantId, savedPayment.CustomerId);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var firstEntry = result[0];
            this.AssertEqual(savedPayment, firstEntry);
            await this.DeleteAll();
        }

        private void AssertEqual(Payment savedPayment, Payment result)
        {
            Assert.Equal(savedPayment.CardNumber, result.CardNumber);
            Assert.Equal(savedPayment.CustomerId, result.CustomerId);
            Assert.Equal(savedPayment.CurrencyCode, result.CurrencyCode);
            Assert.Equal(savedPayment.Id, result.Id);
            Assert.Equal(savedPayment.MerchantId, result.MerchantId);
            Assert.Equal(savedPayment.PaymentDate, result.PaymentDate, TimeSpan.FromMinutes(2));
            Assert.Equal(savedPayment.Status, result.Status);
            Assert.Equal(savedPayment.Value, result.Value);
        }
    }
}