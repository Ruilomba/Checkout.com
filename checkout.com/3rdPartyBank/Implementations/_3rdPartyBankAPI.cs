namespace _3rdPartyBank.Implementations
{
    using System.Threading.Tasks;
    using _3rdPartyBank.DTO.Payments;

    public class _3rdPartyBankAPI : I3rdPartyBankAPI
    {
        public Task<PaymentResponse> Process(PaymentRequest paymentRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}