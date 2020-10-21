namespace Checkout.com.PaymentGateway.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using Checkout.com.AcquiringBank.Business.Services;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Encryption;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories;
    using Checkout.com.PaymentGateway.Business.Services;
    using Checkout.com.PaymentGateway.Business.Services.Implementations;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class PaymentServiceTests
    {
        private readonly PaymentService paymentService;
        private readonly Mock<Business.Adapters.IPaymentProcessor> paymentProcessorMock;
        private readonly Mock<IMerchantService> merchantServiceMock;
        private readonly Mock<IPaymentRepository> paymentRepositoryMock;
        private readonly Mock<IEncryptionService> encryptionServiceMock;
        private readonly ApplicationSettings applicationSettings;
        private readonly Fixture fixture = new Fixture();

        public PaymentServiceTests()
        {
            this.applicationSettings = new ApplicationSettings
            {
                Secret = "1234"
            };
            this.paymentProcessorMock = new Mock<Business.Adapters.IPaymentProcessor>();
            this.merchantServiceMock = new Mock<IMerchantService>();
            this.paymentRepositoryMock = new Mock<IPaymentRepository>();
            this.encryptionServiceMock = new Mock<IEncryptionService>();
            this.paymentService = new PaymentService(
                this.paymentProcessorMock.Object,
                this.merchantServiceMock.Object, 
                this.applicationSettings,
                this.paymentRepositoryMock.Object,
                this.encryptionServiceMock.Object);
        }

        [Fact]
        public async Task GetPaymentById_NoResult_ShouldReturnNull()
        {
            var id = this.fixture.Create<Guid>();
            DTO.Payments.Payment payment = null;
            ArrangeGetById(id, payment);

            var result = await this.paymentService.GetPaymentById(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPaymentById_ValidData_ShouldReturnResultWithCardDecrypted()
        {
            var id = this.fixture.Create<Guid>();
            DTO.Payments.Payment payment = this.fixture.Create<DTO.Payments.Payment>();
            var decryptedCard = this.fixture.Create<string>();
            ArrangeGetById(id, payment);
            ArrangeEncryption(payment, decryptedCard);

            var result = await this.paymentService.GetPaymentById(id);

            result.Should().NotBeNull();
            result.CardNumber.Should().Be(decryptedCard);
            result.CurrencyCode.Should().Be(payment.CurrencyCode);
            result.CustomerId.Should().Be(payment.CustomerId);
            result.PaymentDate.Should().Be(payment.PaymentDate);
            result.Status.Should().Be(payment.Status);
            result.Value.Should().Be(payment.Value);
        }

        private void ArrangeEncryption(DTO.Payments.Payment payment, string decryptedCard)
        {
            this.encryptionServiceMock.Setup(x => x.Decrypt(payment.CardNumber, this.applicationSettings.Secret)).Returns(decryptedCard);
        }

        private void ArrangeGetById(Guid id, DTO.Payments.Payment payment)
        {
            this.paymentRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(payment);
        }
    }
}