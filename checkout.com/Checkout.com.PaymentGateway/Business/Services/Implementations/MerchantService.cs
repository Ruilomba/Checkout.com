namespace Checkout.com.PaymentGateway.Business.Services.Implementations
{
    using System;
    using System.Collections.Generic;

    public class MerchantService : IMerchantService
    {
        public Dictionary<string, decimal> MerchantContractComissions { get; set; }

        public MerchantService()
        {
            this.MerchantContractComissions = new Dictionary<string, decimal>
            {
                {"1", 0.10m },
                {"2", 0.20m },
                {"3", 0.15m },
                {"4", 0.03m },
                {"5", 0.20m },
                {"6", 0.3m },
                {"7", 0.07m },
            };
        }

        public decimal GetCommisionFromMerchant(string merchantId)
        {
            if (!this.MerchantContractComissions.TryGetValue(merchantId, out var commissionPercentage))
            {
                throw new NotSupportedException($"We do not have a contract with merchant {merchantId}");
            }

            return commissionPercentage;
        }
    }
}