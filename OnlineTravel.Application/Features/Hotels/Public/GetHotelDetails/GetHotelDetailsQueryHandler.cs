using Mapster;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Hotels.Dtos;
using OnlineTravel.Application.Features.Hotels.Shared;
using OnlineTravel.Application.Interfaces.Persistence;


namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
	public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, Result<HotelDetailsResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetHotelDetailsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<HotelDetailsResponse>> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
		{
			var hotel = await _unitOfWork.Hotels.GetWithRoomsAsync(request.Id);

			if (hotel == null)
				return Result<HotelDetailsResponse>.Failure("Hotel not found");

			var dto = hotel.Adapt<HotelDetailsResponse>();

			return Result<HotelDetailsResponse>.Success(dto);
		}
	}

}
