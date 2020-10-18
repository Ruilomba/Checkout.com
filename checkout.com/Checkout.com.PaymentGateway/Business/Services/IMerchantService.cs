namespace Checkout.com.PaymentGateway.Business.Services
{
    public interface IMerchantService
    {
        decimal GetCommisionFromMerchant(string merchantId);
    }
}
