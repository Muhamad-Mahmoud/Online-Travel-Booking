using System.Text.RegularExpressions;

namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record EmailAddress
{
	public string Value { get; init; } = string.Empty;

	protected EmailAddress() { } // For EF

	public EmailAddress(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email cannot be empty");
		if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
			throw new ArgumentException("Invalid email format");

		Value = value.ToLowerInvariant();
	}

	public static implicit operator string(EmailAddress email) => email.Value;
	public override string ToString() => Value;
}




