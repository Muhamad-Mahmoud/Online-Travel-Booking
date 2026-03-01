using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Hotels.Shared;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms
{
	public class GetHotelRoomsQuery : IRequest<Result<List<RoomResponse>>>
	{
		public Guid HotelId { get; set; }
		public DateOnly? CheckIn { get; set; }
		public DateOnly? CheckOut { get; set; }
	}

}
