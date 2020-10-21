namespace Checkout.com.Common.Utils
{
    public static class Utils
    {
        public static Money GetPercentageValue(this Money value, decimal percentage)
        {
            return new Money(value.Value - (value.Value / (1 + (percentage / 100))), value.Currency);
        }
    }
}
