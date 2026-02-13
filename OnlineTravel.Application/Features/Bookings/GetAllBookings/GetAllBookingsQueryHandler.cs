using AutoMapper;
using MediatR;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;

using Microsoft.Extensions.Logging;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<IReadOnlyList<BookingResponse>>>
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

    public async Task<Result<IReadOnlyList<BookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Retrieving all bookings (Page {Page}, Size {Size})", request.PageIndex, request.PageSize);

        var spec = new GetAllBookingsSpec(request.PageIndex, request.PageSize);
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

        var response = _mapper.Map<IReadOnlyList<BookingResponse>>(bookings);

        _logger.LogDebug("Retrieved {Count} bookings", bookings.Count);

        return Result<IReadOnlyList<BookingResponse>>.Success(response);
    }
}
