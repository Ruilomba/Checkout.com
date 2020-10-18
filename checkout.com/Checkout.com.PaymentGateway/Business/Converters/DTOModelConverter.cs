namespace Checkout.com.PaymentGateway.Business.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model = DAL.Model;

    public static class DTOModelConverter
    {
        public static DTO.Payments.Payment ToDTO(
            this DTO.Payments.PaymentRequest paymentRequest,
            DTO.Payments.PaymentStatus paymentStatus)
        {
            return new DTO.Payments.Payment
            {
                CardNumber = paymentRequest.Shopper.Card.CardNumber,
                CurrencyCode = paymentRequest.Shopper.User.CurrencyCode,
                CustomerId = paymentRequest.Shopper.User.UserName,
                MerchantId = paymentRequest.Merchant.Id,
                PaymentDate = DateTime.UtcNow,
                Status = paymentStatus,
                Value = paymentRequest.PurchaseValue.Value,
            };
        }

        public static DTO.Payments.Payment ToDTO(this Model.Payment payment)
        {
            return new DTO.Payments.Payment
            {
                CardNumber = payment.CardNumber,
                CurrencyCode = payment.CurrencyCode,
                CustomerId = payment.CustomerId,
                MerchantId = payment.MerchantId,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status.ToDTO(),
                Value = payment.Value
            };
        }

        public static DTO.Payments.PaymentStatus ToDTO(this Model.PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case Model.PaymentStatus.Accepted:
                    return DTO.Payments.PaymentStatus.Accepted;

                case Model.PaymentStatus.Declined:
                    return DTO.Payments.PaymentStatus.Declined;

                default:
                    throw new NotSupportedException("Payment status not recognized");
            }
        }

        public static List<DTO.Payments.Payment> ToDTO(this List<Model.Payment> payments)
        {
            return payments.Select(payment => payment.ToDTO()).ToList();
        }

        public static Model.Payment ToModel(this DTO.Payments.Payment payment)
        {
            return new Model.Payment
            {
                Id = payment.Id,
                CardNumber = payment.CardNumber,
                CurrencyCode = payment.CurrencyCode,
                CustomerId = payment.CustomerId,
                MerchantId = payment.MerchantId,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status.ToModel(),
                Value = payment.Value
            };
        }

        public static Model.PaymentStatus ToModel(this DTO.Payments.PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case DTO.Payments.PaymentStatus.Accepted:
                    return Model.PaymentStatus.Accepted;

                case DTO.Payments.PaymentStatus.Declined:
                    return Model.PaymentStatus.Declined;

                default:
                    throw new NotSupportedException("Payment status not recognized");
            }
        }

        public static List<Model.Payment> ToModel(this List<DTO.Payments.Payment> payments)
        {
            return payments.Select(payment => payment.ToModel()).ToList();
        }
    }
}