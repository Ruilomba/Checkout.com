namespace _3rdPartyBank.DTO.Payments
{
    public class PaymentRequest
    {
        public Payment MerchantPayment { get; set; }

        public Payment CommissionPayment { get; set; }
    }
}