using NetTopologySuite.Geometries;

namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record Address
{
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Country { get; init; }
    public string? PostalCode { get; init; }
    public string? FullAddress { get; init; }
    public Point? Coordinates { get; init; }

    protected Address() { } // For EF

    public Address(string fullAddress, Point? coordinates = null)
    {
        FullAddress = fullAddress;
        Coordinates = coordinates;
    }

    public Address(string? street, string? city, string? state, string? country, string? postalCode, Point? coordinates = null)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
        Coordinates = coordinates;
        FullAddress = string.Join(", ", new[] { street, city, state, country, postalCode }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}




