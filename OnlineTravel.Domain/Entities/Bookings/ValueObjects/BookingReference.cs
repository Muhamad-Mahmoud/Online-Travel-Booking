namespace OnlineTravel.Domain.Entities.Bookings.ValueObjects;

public record BookingReference
{
	public string Value { get; init; } = string.Empty;

	protected BookingReference() { } // For EF

	public BookingReference(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Booking reference cannot be empty");

		Value = value.Trim().ToUpperInvariant();
	}

	public static implicit operator string(BookingReference br) => br.Value;
	public override string ToString() => Value;
}




