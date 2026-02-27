namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record Url
{
	public string Value { get; init; } = string.Empty;

	protected Url() { } // For EF

	public Url(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("URL cannot be empty");

		if (!Uri.TryCreate(value, UriKind.Absolute, out _))
			throw new ArgumentException("Invalid URL format");

		Value = value;
	}

	public static implicit operator string(Url url) => url.Value;
	public override string ToString() => Value;
}




