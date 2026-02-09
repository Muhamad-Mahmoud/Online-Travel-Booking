namespace OnlineTravel.Domain.Entities.Flights.ValueObjects;

public record IataCode
{
    public string Value { get; init; } = string.Empty;

    protected IataCode() { } // For EF

    public IataCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("IATA code cannot be empty");

        string trimmed = value.Trim().ToUpperInvariant();
        if (trimmed.Length < 2 || trimmed.Length > 3)
            throw new ArgumentException("IATA code must be 2 or 3 characters");

        Value = trimmed;
    }

    public static implicit operator string(IataCode code) => code.Value;
    public override string ToString() => Value;
}




