using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, OnlineTravel.Application.Common.Result<AdminBookingResponse>>


{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ILogger<GetBookingByIdQueryHandler> _logger;

	public GetBookingByIdQueryHandler(
		IUnitOfWork unitOfWork,
		IMapper mapper,
		ILogger<GetBookingByIdQueryHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<OnlineTravel.Application.Common.Result<AdminBookingResponse>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Fetching booking details for BookingId {BookingId}", request.BookingId);


		var spec = new GetBookingByIdSpec(request.BookingId);
		var booking = await _unitOfWork.Repository<BookingEntity>().GetEntityWithAsync(spec, cancellationToken);

		if (booking is null)
		{
			_logger.LogWarning("Booking {BookingId} not found", request.BookingId);
			return OnlineTravel.Application.Common.Result<AdminBookingResponse>.Failure("The specified booking does not exist.");
		}


		var response = _mapper.Map<AdminBookingResponse>(booking);

		return OnlineTravel.Application.Common.Result<AdminBookingResponse>.Success(response);
	}
}

