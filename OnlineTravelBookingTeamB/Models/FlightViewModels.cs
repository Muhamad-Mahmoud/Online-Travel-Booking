using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBookingTeamB.Models;

// ───────── Create Flight ─────────
public class CreateFlightViewModel
{
    [Required(ErrorMessage = "Flight number is required")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Carrier is required")]
    public Guid CarrierId { get; set; }

    [Required(ErrorMessage = "Origin airport is required")]
    public Guid OriginAirportId { get; set; }

    [Required(ErrorMessage = "Destination airport is required")]
    public Guid DestinationAirportId { get; set; }

    [Required(ErrorMessage = "Departure time is required")]
    public DateTime DepartureTime { get; set; } = DateTime.Now.AddDays(1);

    [Required(ErrorMessage = "Arrival time is required")]
    public DateTime ArrivalTime { get; set; } = DateTime.Now.AddDays(1).AddHours(3);

    public string BaggageRulesText { get; set; } = string.Empty; // comma-separated
    public bool Refundable { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public Guid CategoryId { get; set; }

    // Metadata
    public string? Gate { get; set; }
    public string? Terminal { get; set; }
    public string? AircraftType { get; set; }
}

// ───────── Edit Flight ─────────
public class EditFlightViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Flight number is required")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Carrier is required")]
    public Guid CarrierId { get; set; }

    [Required(ErrorMessage = "Origin airport is required")]
    public Guid OriginAirportId { get; set; }

    [Required(ErrorMessage = "Destination airport is required")]
    public Guid DestinationAirportId { get; set; }

    [Required] public DateTime DepartureTime { get; set; }
    [Required] public DateTime ArrivalTime { get; set; }

    public string BaggageRulesText { get; set; } = string.Empty;
    public bool Refundable { get; set; }

    [Required] public Guid CategoryId { get; set; }

    // Status
    public string Status { get; set; } = "Scheduled";

    // Metadata
    public string? Gate { get; set; }
    public string? Terminal { get; set; }
    public string? AircraftType { get; set; }
}

// ───────── Manage Flight ─────────
public class ManageFlightViewModel
{
    public OnlineTravel.Domain.Entities.Flights.Flight Flight { get; set; } = null!;
    public AddFlightSeatViewModel SeatForm { get; set; } = new();
    public AddFlightFareViewModel FareForm { get; set; } = new();
}

// ───────── Add Flight Seat ─────────
public class AddFlightSeatViewModel
{
    public Guid FlightId { get; set; }

    [Required(ErrorMessage = "Seat label is required")]
    public string SeatLabel { get; set; } = string.Empty;

    [Required] public string CabinClass { get; set; } = "Economy";

    public string SeatFeaturesText { get; set; } = string.Empty;
    public decimal ExtraCharge { get; set; }
}

// ───────── Add Flight Fare ─────────
public class AddFlightFareViewModel
{
    public Guid FlightId { get; set; }

    [Required]
    [Range(0.01, 99999, ErrorMessage = "Base price must be greater than 0")]
    public decimal BasePrice { get; set; }

    public string Currency { get; set; } = "USD";

    [Required]
    [Range(1, 999, ErrorMessage = "Seats must be at least 1")]
    public int SeatsAvailable { get; set; } = 1;
}

// ───────── Create Airport ─────────
public class CreateAirportViewModel
{
    [Required(ErrorMessage = "IATA code is required")]
    [StringLength(3, MinimumLength = 2, ErrorMessage = "Code must be 2 or 3 characters")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Airport name is required")]
    public string Name { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    [Required] public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    [Required] public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string FacilitiesText { get; set; } = string.Empty; // comma-separated
}

// ───────── Edit Airport ─────────
public class EditAirportViewModel
{
    public Guid Id { get; set; }

    [Required] public string Code { get; set; } = string.Empty;
    [Required] public string Name { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    [Required] public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    [Required] public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string FacilitiesText { get; set; } = string.Empty;
}

// ───────── Create Carrier ─────────
public class CreateCarrierViewModel
{
    [Required(ErrorMessage = "Carrier name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "IATA code is required")]
    [StringLength(3, MinimumLength = 2)]
    public string Code { get; set; } = string.Empty;

    public string? Logo { get; set; }

    [EmailAddress] public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
