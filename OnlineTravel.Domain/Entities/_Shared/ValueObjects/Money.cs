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
}




