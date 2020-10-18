namespace Checkout.com.PaymentGateway.Business.Converters
{
    using System;
    using Checkout.com.Common.Configuration.Entities;
    using Checkout.com.PaymentGateway.Business.Adapters.DTO;

    public static class AcquiringBankInternalExternalAdapters
    {
        public static AcquiringBank.DTO.Payments.PaymentRequest ToExternalRequest(
            this PaymentProcessorPaymentRequest paymentRequest,
            GatewayCard gatewayCard,
            decimal commissionPaymentValue,
            decimal merchantPaymentValue)
        {
            if (paymentRequest == null)
            {
                return null;
            }

            return new AcquiringBank.DTO.Payments.PaymentRequest
            {
                CommissionPayment = new AcquiringBank.DTO.Payments.Payment
                {
                    CardToDeposit = new AcquiringBank.DTO.Card.Card
                    {
                        CardNumber = gatewayCard.CardNumber,
                        CCV = gatewayCard.CCV,
                        ExpirationDate = new AcquiringBank.DTO.Card.ExpirationDate
                        {
                            Month = gatewayCard.ExpirationDate.Month,
                            Year = gatewayCard.ExpirationDate.Year
                        }
                    },
                    CardToWithraw = paymentRequest.Shopper.Card.ToExternalRequest(),
                    CurrencyCode = paymentRequest.Shopper.User.CurrencyCode,
                    Value = commissionPaymentValue
                },
                MerchantPayment = new AcquiringBank.DTO.Payments.Payment
                {
                    CardToDeposit = paymentRequest.Merchant.Card.ToExternalRequest(),
                    CardToWithraw = paymentRequest.Shopper.Card.ToExternalRequest(),
                    CurrencyCode = paymentRequest.Merchant.CurrencyCode,
                    Value = merchantPaymentValue
                }
            };
        }

        public static AcquiringBank.DTO.Card.Card ToExternalRequest(this DTO.Card.Card card)
        {
            if(card == null)
            {
                return null;
            }

            return new AcquiringBank.DTO.Card.Card
            {
                CardNumber = card.CardNumber,
                CCV = card.CCV,
                ExpirationDate = new AcquiringBank.DTO.Card.ExpirationDate
                {
                    Month = card.ExpirationDate.Month,
                    Year = card.ExpirationDate.Year
                }
            };
        }

        public static DTO.Payments.PaymentResponse ToInternalDTO(this AcquiringBank.DTO.Payments.PaymentResponse paymentResponse)
        {
            if (paymentResponse == null)
            {
                return null;
            }

            return new DTO.Payments.PaymentResponse
            {
                PaymentStatus = paymentResponse.PaymentStatus.ToInternalDTO()
            };
        }

        public static DTO.Payments.PaymentStatus ToInternalDTO(this AcquiringBank.DTO.Payments.PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case AcquiringBank.DTO.Payments.PaymentStatus.Accepted:
                    return DTO.Payments.PaymentStatus.Accepted;
                case AcquiringBank.DTO.Payments.PaymentStatus.Declined:
                    return DTO.Payments.PaymentStatus.Declined;
                default:
                    throw new NotSupportedException("Payment status not recognized");
            }
        }
    }
}