using AutoMapper;
using MediatR;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<IReadOnlyList<BookingResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<BookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllBookingsSpec(request.PageIndex, request.PageSize);
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

        var response = _mapper.Map<IReadOnlyList<BookingResponse>>(bookings);

        return Result<IReadOnlyList<BookingResponse>>.Success(response);
    }
}
