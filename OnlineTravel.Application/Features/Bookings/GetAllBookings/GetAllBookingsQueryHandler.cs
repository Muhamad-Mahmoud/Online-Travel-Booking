using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.Helpers;
using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<PagedResult<AdminBookingResponse>>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetAllBookingsQueryHandler> _logger;

	public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllBookingsQueryHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task<Result<PagedResult<AdminBookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)

	{
		_logger.LogDebug("Retrieving all bookings (Page {Page}, Size {Size})", request.PageIndex, request.PageSize);

		// Create Count Specification (isCount: true)
		var countSpec = new GetAllBookingsSpec(request.PageIndex, request.PageSize, request.SearchTerm, request.Status, isCount: true);
		var totalCount = await _unitOfWork.Repository<BookingEntity>().GetCountAsync(countSpec, cancellationToken);

		// Create Data Specification (isCount: false - default)
		var spec = new GetAllBookingsSpec(request.PageIndex, request.PageSize, request.SearchTerm, request.Status);
		var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

		// Handle lazy expiration
		if (BookingExpirationHelper.MarkExpiredBookings(bookings))
		{
			await _unitOfWork.Complete();
		}

		var bookingDtos = bookings.Adapt<IReadOnlyList<AdminBookingResponse>>();

		_logger.LogDebug("Retrieved {Count} bookings", bookings.Count);

		var pagedResult = new PagedResult<AdminBookingResponse>(bookingDtos, totalCount, request.PageIndex, request.PageSize);
		return Result<PagedResult<AdminBookingResponse>>.Success(pagedResult);
	}
}
