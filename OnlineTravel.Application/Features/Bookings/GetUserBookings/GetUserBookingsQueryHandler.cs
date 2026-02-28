using AutoMapper;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;


namespace OnlineTravel.Application.Features.Bookings.GetUserBookings
{
	public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, OnlineTravel.Application.Common.Result<PagedResult<AdminBookingResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		public async Task<OnlineTravel.Application.Common.Result<PagedResult<AdminBookingResponse>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
		{
			// Simplification for recovery
			var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllAsync();
			var userBookings = bookings.Where(b => b.UserId == request.UserId).ToList();
			
			var data = _mapper.Map<List<AdminBookingResponse>>(userBookings.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize));


			return OnlineTravel.Application.Common.Result<PagedResult<AdminBookingResponse>>.Success(new PagedResult<AdminBookingResponse>(data, userBookings.Count, request.PageIndex, request.PageSize));
		}
	}
}

