using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour;

public class UpdateTourCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<bool>>
{
    public Guid TourId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public bool Recommended { get; set; }
    public string? BestTimeToVisit { get; set; }

    // Address
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    // Main Image (only updated if not null/empty)
    public string? MainImageUrl { get; set; }
}

