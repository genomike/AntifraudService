using System;

namespace AntifraudService.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            }

            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Money other)
            {
                return Amount == other.Amount && Currency == other.Currency;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}