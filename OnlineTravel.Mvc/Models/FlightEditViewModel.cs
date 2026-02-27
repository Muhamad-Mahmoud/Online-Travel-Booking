namespace OnlineTravel.Mvc.Models;

public class FlightEditViewModel
{
    public Guid Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public Guid? CarrierId { get; set; }
    public Guid? CategoryId { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Refundable { get; set; }
    public Guid? OriginAirportId { get; set; }
    public Guid? DestinationAirportId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string? Gate { get; set; }
    public string? Terminal { get; set; }
    public string? AircraftType { get; set; }
    public List<string> BaggageRules { get; set; } = new();
    public string? BaggageRulesText { get; set; }
}
