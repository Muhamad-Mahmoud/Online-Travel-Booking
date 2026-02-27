namespace OnlineTravel.Domain.Entities.Reviews.ValueObjects;

public record StarRating
{
	public decimal Value { get; init; }

	protected StarRating() { } // For EF

	public StarRating(decimal value)
	{
		if (value < 1 || value > 5) throw new ArgumentException("Star rating must be between 1 and 5");
		Value = value;
	}

	public static implicit operator decimal(StarRating rating) => rating.Value;
}




