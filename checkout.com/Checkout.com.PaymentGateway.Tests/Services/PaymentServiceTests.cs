namespace Checkout.com.PaymentGateway.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
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
            ArrangeDecryption(payment, decryptedCard);

            var result = await this.paymentService.GetPaymentById(id);

            result.Should().NotBeNull();
            this.AssertEqualPayment(payment, decryptedCard, result);
        }


        [Fact]
        public async Task SearchPayment_NoResults_ShouldReturnEmpty()
        {
            var cardNumber = this.fixture.Create<string>();
            var customerId = this.fixture.Create<string>();
            var merchantId = this.fixture.Create<string>();
            List<DTO.Payments.Payment> payments = Enumerable.Empty<DTO.Payments.Payment>().ToList();
            var encryptedCard = this.fixture.Create<string>();
            ArrangeEncryption(cardNumber, encryptedCard);
            ArrangeSearchQuery(encryptedCard, customerId, merchantId, payments);

            var result = await this.paymentService.SearchPayments(cardNumber, merchantId, customerId);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchPayment_WithResults_ShouldReturnResultWithDecryptedCard()
        {
            var cardNumber = this.fixture.Create<string>();
            var customerId = this.fixture.Create<string>();
            var merchantId = this.fixture.Create<string>();
            var encryptedCard = this.fixture.Create<string>();
            List<DTO.Payments.Payment> payments = this.fixture.Build<DTO.Payments.Payment>()
                .With(x => x.CardNumber, encryptedCard)
                .CreateMany(1)
                .ToList();

            ArrangeEncryption(cardNumber, encryptedCard);
            ArrangeSearchQuery(encryptedCard, customerId, merchantId, payments);
            payments.ForEach(x => ArrangeDecryption(x, cardNumber));

            var result = await this.paymentService.SearchPayments(cardNumber, merchantId, customerId);

            result.Should().NotBeEmpty()
                .And
                .HaveCount(1)
                .And
                .SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(payments[0].Id);
                    this.AssertEqualPayment(first, cardNumber, payments[0]);
                });
        }

        [Fact]
        public async Task ProcessPayment_ValidData_ShouldStoreInDatabaseAndReturn()
        {
            var merchantCardNumber = this.fixture.Create<int>().ToString();
            var merchantCardCCV = this.fixture.Create<int>().ToString();
            var shopperCardNumber = this.fixture.Create<int>().ToString();
            var shopperCardCCV = this.fixture.Create<int>().ToString();
            var nextYear = DateTime.Now.AddYears(1).Year;
            DTO.Payments.PaymentRequest paymentRequest = InitializePaymentRequest(
                merchantCardNumber,
                merchantCardCCV,
                shopperCardNumber,
                shopperCardCCV, 
                nextYear);
            var encryptedShopperCardNumber = this.fixture.Create<string>();
            var encryptedShopperCardCCV = this.fixture.Create<string>();
            var encryptedMerchantCardNumber = this.fixture.Create<string>();
            var encryptedMerchantCardCCV = this.fixture.Create<string>();
            var merchantCommission = this.fixture.Create<decimal>();
            var processmentResult = this.fixture.Create<DTO.Payments.PaymentResponse>();
            var paymentSaveResult = this.fixture.Create<DTO.Payments.Payment>();
            var paymentSaveResultDecryptedCard = this.fixture.Create<string>();

            this.Arrange(
                paymentRequest,
                encryptedShopperCardNumber,
                encryptedShopperCardCCV,
                encryptedMerchantCardNumber,
                encryptedMerchantCardCCV,
                merchantCommission,
                processmentResult,
                paymentSaveResult,
                paymentSaveResultDecryptedCard);

            var result = await this.paymentService.ProcessPayment(paymentRequest);

            AssertEqualPayment(paymentSaveResult, paymentSaveResultDecryptedCard, result);
        }

        [Fact]
        public async Task ProcessPayment_InvalidDataCardExpired_ShouldThrow()
        {
            var merchantCardNumber = this.fixture.Create<int>().ToString();
            var merchantCardCCV = this.fixture.Create<int>().ToString();
            var shopperCardNumber = this.fixture.Create<int>().ToString();
            var shopperCardCCV = this.fixture.Create<int>().ToString();
            var prevYear = DateTime.Now.AddYears(-1).Year;
            DTO.Payments.PaymentRequest paymentRequest = InitializePaymentRequest(
                merchantCardNumber,
                merchantCardCCV,
                shopperCardNumber,
                shopperCardCCV,
                prevYear);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await this.paymentService.ProcessPayment(paymentRequest));
            Assert.Equal("Merchant Card is expired", ex.Message);
        }

        [Fact]
        public async Task ProcessPayment_InvalidDataCardNotValid_ShouldThrow()
        {
            var merchantCardNumber = this.fixture.Create<string>();
            var merchantCardCCV = this.fixture.Create<int>().ToString();
            var shopperCardNumber = this.fixture.Create<int>().ToString();
            var shopperCardCCV = this.fixture.Create<int>().ToString();
            var prevYear = DateTime.Now.AddYears(-1).Year;
            DTO.Payments.PaymentRequest paymentRequest = InitializePaymentRequest(
                merchantCardNumber,
                merchantCardCCV,
                shopperCardNumber,
                shopperCardCCV,
                prevYear);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await this.paymentService.ProcessPayment(paymentRequest));
            Assert.Equal("Merchant Card number is not valid", ex.Message);
        }

        private DTO.Payments.PaymentRequest InitializePaymentRequest(string merchantCardNumber, string merchantCardCCV, string shopperCardNumber, string shopperCardCCV, int nextYear)
        {
            var paymentRequest = this.fixture.Build<DTO.Payments.PaymentRequest>()
                            .Create();
            paymentRequest.Merchant.Card.CardNumber = merchantCardNumber;
            paymentRequest.Merchant.Card.CCV = merchantCardCCV;
            paymentRequest.Shopper.Card.CCV = shopperCardCCV;
            paymentRequest.Shopper.Card.CardNumber = shopperCardNumber;
            paymentRequest.Merchant.Card.ExpirationDate.Year = nextYear;
            paymentRequest.Shopper.Card.ExpirationDate.Year = nextYear;
            return paymentRequest;
        }

        private void Arrange(DTO.Payments.PaymentRequest paymentRequest, string encryptedShopperCardNumber, string encryptedShopperCardCCV, string encryptedMerchantCardNumber, string encryptedMerchantCardCCV, decimal merchantCommission, DTO.Payments.PaymentResponse processmentResult, DTO.Payments.Payment paymentSaveResult, string paymentSaveResultDecryptedCard)
        {
            ArrangeEncryption(paymentRequest.Shopper.Card.CardNumber, encryptedShopperCardNumber);
            ArrangeEncryption(paymentRequest.Shopper.Card.CCV, encryptedShopperCardCCV);
            ArrangeEncryption(paymentRequest.Shopper.Card.CardNumber, encryptedMerchantCardNumber);
            ArrangeEncryption(paymentRequest.Merchant.Card.CCV, encryptedMerchantCardCCV);

            this.merchantServiceMock.Setup(x => x.GetCommisionFromMerchant(paymentRequest.Merchant.Id)).Returns(merchantCommission);

            this.paymentProcessorMock.Setup(x => x.ProcessPayment(
                It.Is<Business.Adapters.DTO.PaymentProcessorPaymentRequest>(x =>
                        x.GatewayCommission == merchantCommission
                        && x.Merchant.Equals(paymentRequest.Merchant)
                        && x.Shopper.Equals(paymentRequest.Shopper)
                        && x.PurchaseValue.Currency == paymentRequest.PurchaseValue.CurrencyCode
                        && x.PurchaseValue.Value == paymentRequest.PurchaseValue.Value
                    ))).ReturnsAsync(processmentResult);

            this.paymentRepositoryMock.Setup(x => x.SavePayment(It.Is<DTO.Payments.Payment>(x =>
                    x.CardNumber == paymentRequest.Shopper.Card.CardNumber
                    && x.CurrencyCode == paymentRequest.Shopper.User.CurrencyCode
                    && x.CustomerId == paymentRequest.Shopper.User.UserName
                    && x.MerchantId == paymentRequest.Merchant.Id
                    && x.Status == processmentResult.PaymentStatus
                    && x.Value == paymentRequest.PurchaseValue.Value
                    && x.PaymentDate != null
                ))).ReturnsAsync(paymentSaveResult);

            ArrangeDecryption(paymentSaveResult, paymentSaveResultDecryptedCard);
        }

        private void AssertEqualPayment(DTO.Payments.Payment payment, string expectedCardNumber, DTO.Payments.Payment result)
        {
            result.CardNumber.Should().Be(expectedCardNumber);
            result.CurrencyCode.Should().Be(payment.CurrencyCode);
            result.CustomerId.Should().Be(payment.CustomerId);
            result.PaymentDate.Should().Be(payment.PaymentDate);
            result.Status.Should().Be(payment.Status);
            result.Value.Should().Be(payment.Value);
            result.MerchantId.Should().Be(payment.MerchantId);
        }

        private void ArrangeEncryption(string cardNumber, string encryptedCard)
        {
            this.encryptionServiceMock.Setup(x => x.Encrypt(cardNumber, this.applicationSettings.Secret)).Returns(encryptedCard);
        }

        private void ArrangeSearchQuery(string cardNumber, string customerId, string merchantId, List<DTO.Payments.Payment> payments)
        {
            this.paymentRepositoryMock.Setup(x => x.Search(cardNumber, merchantId, customerId)).ReturnsAsync(payments);
        }

        private void ArrangeDecryption(DTO.Payments.Payment payment, string decryptedCard)
        {
            this.encryptionServiceMock.Setup(x => x.Decrypt(payment.CardNumber, this.applicationSettings.Secret)).Returns(decryptedCard);
        }

        private void ArrangeGetById(Guid id, DTO.Payments.Payment payment)
        {
            this.paymentRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(payment);
        }
    }
}