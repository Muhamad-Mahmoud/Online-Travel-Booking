namespace OnlineTravel.Mvc.Models;

public class FlightManageViewModel
{
    public FlightDto Flight { get; set; } = new();
    public SeatFormDto SeatForm { get; set; } = new();
    public FareFormDto FareForm { get; set; } = new();
}

public class FlightDto
{
    public Guid Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public CarrierDto? Carrier { get; set; }
    public FlightScheduleDto Schedule { get; set; } = new();
    public decimal BaseFare { get; set; }
    public int AvailableSeats { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AircraftType { get; set; }
    public DateTime? LastUpdated { get; set; }
    public bool Refundable { get; set; }
    public AirportDto? OriginAirport { get; set; }
    public AirportDto? DestinationAirport { get; set; }
    public List<SeatDto> Seats { get; set; } = new();
    public List<FareDto> Fares { get; set; } = new();
    public FlightMetadataDto? Metadata { get; set; }
    public string BaggageRules { get; set; } = string.Empty;
}

public class AirportDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class SeatDto
{
    public Guid Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string SeatLabel { get; set; } = string.Empty;
    public string CabinClass { get; set; } = string.Empty;
    public decimal ExtraCharge { get; set; }
    public bool IsAvailable { get; set; }
}

public class FareDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public int SeatsAvailable { get; set; }
}

public class FlightMetadataDto
{
    public string Gate { get; set; } = string.Empty;
    public string Terminal { get; set; } = string.Empty;
}

public class FlightScheduleDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class CarrierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
}

public class SeatFormDto
{
    public Guid Id { get; set; }
    public Guid FlightId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string SeatLabel { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string CabinClass { get; set; } = string.Empty;
    public decimal ExtraCharge { get; set; }
    public string SeatFeaturesText { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}

public class FareFormDto
{
    public Guid Id { get; set; }
    public Guid FlightId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public int SeatsAvailable { get; set; }
}
