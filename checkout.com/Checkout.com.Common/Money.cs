using System;
using System.Collections.Generic;

namespace Checkout.com.Common
{
    public struct Money : IEqualityComparer<Money>, IEquatable<Money>
    {
        public Money(decimal value, string currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public string Currency { get; }

        public decimal Value { get; }

        public static bool operator ==(Money x, Money y) => x.Equals(y);

        public static bool operator !=(Money x, Money y) => !(x == y);

        public static Money operator -(Money x, Money y) =>
            x.Currency == y.Currency ? new Money(x.Value - y.Value, x.Currency) : throw new InvalidOperationException();

        public override bool Equals(object obj) => obj != null && obj is Money && Equals((Money)obj);

        public bool Equals(Money x, Money y) => x.Value == y.Value && x.Currency.Equals(y.Currency);

        public bool Equals(Money other) => this.Value == other.Value && this.Currency == other.Currency;

        public override int GetHashCode()
        {
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 23 + this.Value.GetHashCode();
                hash = hash * 23 + this.Currency.GetHashCode();
                return hash;
            }
        }

        public int GetHashCode(Money obj) => obj.GetHashCode();

        public override string ToString() => $"{this.Value} {this.Currency}";
    }
}