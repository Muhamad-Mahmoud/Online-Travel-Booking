namespace OnlineTravel.Domain.Entities.Flights.ValueObjects;

public record FlightNumber
{
	public string Value { get; init; } = string.Empty;

	protected FlightNumber() { } // For EF

	public FlightNumber(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Flight number cannot be empty");

		Value = value.Trim().ToUpperInvariant();
	}

	public static implicit operator string(FlightNumber num) => num.Value;
	public override string ToString() => Value;
}




