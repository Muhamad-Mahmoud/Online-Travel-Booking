using AutoMapper;
using MediatR;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;
using Microsoft.Extensions.Logging;
using DomainResult = OnlineTravel.Domain.ErrorHandling.Result<OnlineTravel.Application.Common.PagedResult<OnlineTravel.Application.Features.Bookings.Shared.DTOs.AdminBookingResponse>>;

namespace OnlineTravel.Application.Features.Bookings.GetUserBookings;

public sealed class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, OnlineTravel.Domain.ErrorHandling.Result<OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>>>
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

    public async Task<OnlineTravel.Domain.ErrorHandling.Result<OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Retrieving bookings for User {UserId} (page {Page}, size {Size})",
            request.UserId, request.PageIndex, request.PageSize);

        // Count and data fetched with separate specs to keep includes only on the data query
        var countSpec = new GetAllBookingsByUserIdSpec(request.UserId, isCountOnly: true);
        var totalCount = await _unitOfWork.Repository<BookingEntity>().GetCountAsync(countSpec, cancellationToken);

        var dataSpec = new GetAllBookingsByUserIdSpec(request.UserId, request.PageIndex, request.PageSize);
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(dataSpec, cancellationToken);

        var dtos = _mapper.Map<IReadOnlyList<AdminBookingResponse>>(bookings);

        _logger.LogDebug("Found {Total} bookings for User {UserId}", totalCount, request.UserId);

        return OnlineTravel.Domain.ErrorHandling.Result<OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>>.Success(
            new OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>(dtos, totalCount, request.PageIndex, request.PageSize));
    }
}
