using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Dtos;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
	public class GetHotelDetailsQuery : IRequest<Result<HotelDetailsDto>>
	{
		public Guid Id { get; set; }
	}

}
