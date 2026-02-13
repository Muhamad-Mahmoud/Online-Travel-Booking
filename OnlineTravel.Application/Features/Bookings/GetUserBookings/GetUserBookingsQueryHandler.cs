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

namespace OnlineTravel.Application.Features.Bookings.GetUserBookings;

public sealed class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Result<IReadOnlyList<BookingResponse>>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserBookingsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<BookingResponse>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllBookingsByUserIdSpec(request.UserId);
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

        var response = _mapper.Map<IReadOnlyList<BookingResponse>>(bookings);

        return Result<IReadOnlyList<BookingResponse>>.Success(response);
    }
}
