namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record ImageUrl
{
    public string Url { get; init; }
    public string? AltText { get; init; }

    protected ImageUrl() { } // For EF Core

    public ImageUrl(string url, string? altText = null)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be empty");

        Url = url;
        AltText = altText;
    }

    public static implicit operator string(ImageUrl imageUrl) => imageUrl.Url;
    public static implicit operator ImageUrl(string url) => new(url);
}
