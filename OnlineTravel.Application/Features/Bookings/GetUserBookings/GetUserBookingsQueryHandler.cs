using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Features.Bookings.Strategies;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.ErrorHandling;
using Microsoft.Extensions.Logging;

namespace OnlineTravel.Application.Features.Bookings.GetUserBookings;

public sealed class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Result<IReadOnlyList<AdminBookingResponse>>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserBookingsQueryHandler> _logger;

    public GetUserBookingsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetUserBookingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<AdminBookingResponse>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Retrieving bookings for User {UserId}", request.UserId);

        var spec = new GetAllBookingsByUserIdSpec(request.UserId);
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

        var response = _mapper.Map<IReadOnlyList<AdminBookingResponse>>(bookings);

        _logger.LogDebug("Found {Count} bookings for User {UserId}", bookings.Count, request.UserId);

        return Result<IReadOnlyList<AdminBookingResponse>>.Success(response);
    }
}
