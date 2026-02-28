using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Dtos;
using OnlineTravel.Application.Features.Hotels.Shared;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
	public class GetHotelDetailsQuery : IRequest<Result<HotelDetailsResponse>>
	{
		public Guid Id { get; set; }
	}

}
