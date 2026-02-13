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

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookingByIdSpec(request.BookingId);
        var booking = await _unitOfWork.Repository<BookingEntity>().GetEntityWithAsync(spec, cancellationToken);

        if (booking is null)
        {
            return Result<BookingResponse>.Failure(Error.NotFound("The specified booking does not exist."));
        }

        var response = _mapper.Map<BookingResponse>(booking);

        return Result<BookingResponse>.Success(response);
    }
}
