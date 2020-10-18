namespace _3rdPartyBank
{
    using System.Threading.Tasks;
    using _3rdPartyBank.DTO.Payments;

    public interface I3rdPartyBankAPI
    {
        Task<PaymentResponse> Process(PaymentRequest paymentRequest);
    }
}