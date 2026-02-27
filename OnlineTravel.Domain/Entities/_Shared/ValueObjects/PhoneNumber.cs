namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record PhoneNumber
{
	public string Value { get; init; } = string.Empty;

	protected PhoneNumber() { } // For EF

	public PhoneNumber(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Phone number cannot be empty");
		// Simple validation, can be enhanced with regex for E.164
		Value = value;
	}

	public static implicit operator string(PhoneNumber phone) => phone.Value;
	public override string ToString() => Value;
}




