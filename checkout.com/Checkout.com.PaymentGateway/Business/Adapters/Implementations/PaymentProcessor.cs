namespace Checkout.com.PaymentGateway.Business.Adapters.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.com.Common.Configuration;
    using Checkout.com.PaymentGateway.Business.Adapters;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;
    using Checkout.com.PaymentGateway.DTO.Card;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly Dictionary<CardType, IPaymentProcessorAdapter> paymentProcessorAdapterResolver;

        public PaymentProcessor(
            AcquiringBank.Client.IAcquiringBankClient paymentProcessor,
            ApplicationSettings applicationSettings)
        {
            this.paymentProcessorAdapterResolver = new Dictionary<CardType, IPaymentProcessorAdapter>()
            {
                {CardType.MasterCard, new MasterCardProcessorAdapter(paymentProcessor, applicationSettings)},
                {CardType.Visa, new VisaProcessorAdapter()}
            };
        }

        public async Task<PaymentResponse> ProcessPayment(PaymentProcessorPaymentRequest paymentRequest)
        {
            switch (paymentRequest.Shopper.GetCardType())
            {
                case CardType.MasterCard:
                    return await this.paymentProcessorAdapterResolver[CardType.MasterCard].ProcessPayment(paymentRequest);
                case CardType.Visa:
                    return await this.paymentProcessorAdapterResolver[CardType.Visa].ProcessPayment(paymentRequest);
                default:
                    throw new NotSupportedException("Card type not recognized");
            }
        }

    }
}