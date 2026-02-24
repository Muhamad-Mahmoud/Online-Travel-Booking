using AutoMapper;
using MediatR;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;
using Microsoft.Extensions.Logging;

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<AdminBookingResponse>>
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

    public async Task<Result<AdminBookingResponse>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching booking details for BookingId {BookingId}", request.BookingId);

        var spec = new GetBookingByIdSpec(request.BookingId);
        var booking = await _unitOfWork.Repository<BookingEntity>().GetEntityWithAsync(spec, cancellationToken);

        if (booking is null)
        {
            _logger.LogWarning("Booking {BookingId} not found", request.BookingId);
            return Result<AdminBookingResponse>.Failure(Error.NotFound("The specified booking does not exist."));
        }

        var response = _mapper.Map<AdminBookingResponse>(booking);

        return Result<AdminBookingResponse>.Success(response);
    }
}
