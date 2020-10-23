using System.Collections.Generic;

namespace Checkout.com.PaymentGateway
{
    public class User
    {
        public string UserName{ get; set; }

        public string CurrencyCode { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   UserName == user.UserName &&
                   CurrencyCode == user.CurrencyCode;
        }

        public override int GetHashCode()
        {
            int hashCode = -723851476;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrencyCode);
            return hashCode;
        }
    }
}
