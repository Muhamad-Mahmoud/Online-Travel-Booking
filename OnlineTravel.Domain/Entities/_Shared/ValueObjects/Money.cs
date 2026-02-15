using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record Money
{
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; init; }

    public string Currency { get; init; } = "USD";

    protected Money() { } // For EF

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(string currency = "USD") => new(0, currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot add money with different currencies: {left.Currency} and {right.Currency}");
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot subtract money with different currencies: {left.Currency} and {right.Currency}");
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money left, int multiplier)
    {
        return new Money(left.Amount * multiplier, left.Currency);
    }

    public static Money operator *(Money left, decimal multiplier)
    {
        return new Money(left.Amount * multiplier, left.Currency);
    }

    #region
    public Money Add(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");

        return new Money(Amount + other.Amount, Currency);
    }
    #endregion
}




