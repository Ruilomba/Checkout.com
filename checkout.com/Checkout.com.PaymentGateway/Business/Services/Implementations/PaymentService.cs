namespace Checkout.com.PaymentGateway.Business.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Utils;
    using Checkout.com.PaymentGateway.Business.Adapters;
    using Checkout.com.PaymentGateway.Business.Converters;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories;
    using Checkout.com.PaymentGateway.DTO.Card;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentProcessor paymentProcessor;
        private readonly IMerchantService merchantService;
        private readonly ApplicationSettings applicationSettings;
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(
            IPaymentProcessor paymentProcessor, 
            IMerchantService merchantService,
            ApplicationSettings applicationSettings,
            IPaymentRepository paymentRepository)
        {
            this.paymentProcessor = paymentProcessor;
            this.merchantService = merchantService;
            this.applicationSettings = applicationSettings;
            this.paymentRepository = paymentRepository;
        }

        public async Task<Payment> GetPaymentById(Guid id)
        {
            var result = await this.paymentRepository.GetById(id);

            if(result == null)
            {
                return null;
            }

            result.CardNumber.DecryptString(this.applicationSettings.Secret);
            return result;
        }

        public async Task<List<Payment>> SearchPayments(string cardNumber, string merchantId, string customerId)
        {
            var result = await this.paymentRepository.Search(cardNumber, merchantId, customerId);
            result?.ForEach(payment => payment.CardNumber.DecryptString(this.applicationSettings.Secret));
            return result;
        }

        public async Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            this.ValidateRequest(paymentRequest);
            this.EncryptCard(paymentRequest.Merchant.Card);
            this.EncryptCard(paymentRequest.Shopper.Card);

            var commissionContractValue = this.merchantService.GetCommisionFromMerchant(paymentRequest.Merchant.Id);
            var processmentResult =  await this.paymentProcessor.ProcessPayment(paymentRequest.ToAdapterDTO(commissionContractValue));
            var paymentSaveResult = await this.paymentRepository.SavePayment(paymentRequest.ToDTO(processmentResult.PaymentStatus));

            return new PaymentResponse
            {
                PaymentStatus = processmentResult.PaymentStatus,
                PaymentId = paymentSaveResult.Id
            };

        }

        private void EncryptCard(Card card)
        {
            card.CardNumber = card.CardNumber.EncryptString(applicationSettings.Secret);
            card.CCV = card.CardNumber.EncryptString(applicationSettings.Secret);
        }

        private void ValidateRequest(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Merchant == null)
            {
                throw new ArgumentException("Merchant cannot be null");
            }

            if (paymentRequest.Shopper == null)
            {
                throw new ArgumentException("Shopper cannot be null");
            }

            this.ValidateCard(paymentRequest.Merchant.Card);
            this.ValidateCard(paymentRequest.Shopper.Card);

            if (paymentRequest.Shopper.User.UserName == null)
            {
                throw new ArgumentException("Shopper userName cannot be null");
            }

            if(paymentRequest.PurchaseValue == null)
            {
                throw new ArgumentException("Purchase value cannot be null");
            }

            if (paymentRequest.PurchaseValue.CurrencyCode == null)
            {
                throw new ArgumentException("Purchase currency cannot be null");
            }

            if (paymentRequest.PurchaseValue.Value <= 0m)
            {
                throw new ArgumentException("Purchase value cannot be 0 or less");
            }
        }

        private void ValidateCard(Card card)
        {
            if (card == null)
            {
                throw new ArgumentException("Merchant Card cannot be null");
            }

            if (card.CardNumber == null)
            {
                throw new ArgumentException("Merchant Card number cannot be null");
            }

            if (!int.TryParse(card.CardNumber.Trim(), out _))
            {
                throw new ArgumentException("Merchant Card number is not valid");
            }

            if (card.CCV == null)
            {
                throw new ArgumentException("Merchant Card CCV cannot be null");
            }

            if (!int.TryParse(card.CCV, out _))
            {
                throw new ArgumentException("Merchant Card CCV is not valid");
            }

            if (card.ExpirationDate == null)
            {
                throw new ArgumentException("Merchant Card Expiration Date cannot be null");
            }

            var dateTimeNow = DateTime.UtcNow;

            if (card.ExpirationDate.Year > dateTimeNow.Year
                || (card.ExpirationDate.Year == dateTimeNow.Year
                && card.ExpirationDate.Month > dateTimeNow.Month))
            {
                throw new ArgumentException("Merchant Card is expired");
            }
        }
    }
}