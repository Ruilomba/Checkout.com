namespace Checkout.com.PaymentGateway.Business.Converters
{
    using Checkout.com.Common;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;
    using Checkout.com.PaymentGateway.DTO.Payments;

    public static class InternalToAdapterConverted
    {
        public static PaymentProcessorPaymentRequest ToAdapterDTO(
            this PaymentRequest paymentRequest,
            decimal merchantCommision)
        {
            if (paymentRequest == null)
            {
                return null;
            }

            return new PaymentProcessorPaymentRequest
            {
                Merchant = paymentRequest.Merchant,
                PurchaseValue = new Money(paymentRequest.PurchaseValue.Value, paymentRequest.PurchaseValue.CurrencyCode),
                Shopper = paymentRequest.Shopper,
                GatewayCommission = merchantCommision
            };
        }
    }
}