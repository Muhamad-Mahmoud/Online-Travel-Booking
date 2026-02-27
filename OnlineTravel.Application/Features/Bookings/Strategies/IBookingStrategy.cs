using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public interface IBookingStrategy
{
	CategoryType Type { get; }
	Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange? stayRange, CancellationToken cancellationToken);
}
