namespace _3rdPartyBank.Implementations
{
    using System.Threading.Tasks;
    using _3rdPartyBank.DTO.Payments;

    public class _3rdPartyBankAPI : I3rdPartyBankAPI
    {
        public Task<PaymentResponse> Process(PaymentRequest paymentRequest)
        {
            if (paymentRequest.MerchantPayment.CardToWithraw.CardNumber.StartsWith("1"))
            {
                return Task.Run(() => new PaymentResponse
                {
                    PaymentStatus = PaymentStatus.Accepted,
                });
            }

            return Task.Run(() => new PaymentResponse
            {
                PaymentStatus = PaymentStatus.Declined,
            });
        }
    }
}