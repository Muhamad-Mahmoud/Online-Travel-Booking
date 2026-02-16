using AutoMapper;
using MediatR;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Features.Bookings.Helpers;

using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, OnlineTravel.Domain.ErrorHandling.Result<PagedResult<AdminBookingResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllBookingsQueryHandler> _logger;

    public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllBookingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OnlineTravel.Domain.ErrorHandling.Result<PagedResult<AdminBookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
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

        var bookingDtos = _mapper.Map<IReadOnlyList<AdminBookingResponse>>(bookings);

        _logger.LogDebug("Retrieved {Count} bookings", bookings.Count);

        var pagedResult = new PagedResult<AdminBookingResponse>(bookingDtos, totalCount, request.PageIndex, request.PageSize);
        return OnlineTravel.Domain.ErrorHandling.Result<PagedResult<AdminBookingResponse>>.Success(pagedResult);
    }
}
