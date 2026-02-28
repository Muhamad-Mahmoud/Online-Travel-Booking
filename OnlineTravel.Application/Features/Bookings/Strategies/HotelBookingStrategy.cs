using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.Strategies
{
	public class HotelBookingStrategy : IBookingStrategy
	{
		private readonly IUnitOfWork _unitOfWork;
		public CategoryType Type => CategoryType.Hotel;

		public HotelBookingStrategy(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

		public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange? stayRange, CancellationToken ct)
		{
			var range = stayRange ?? new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));
			// Simplification for recovery
			return Result<BookingProcessResult>.Success(new BookingProcessResult(
				new Money(100),
				"Hotel Room",
				range
			));
		}
	}

}

