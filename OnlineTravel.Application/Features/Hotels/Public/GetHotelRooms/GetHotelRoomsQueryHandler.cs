using Mapster;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms
{
	public class GetHotelRoomsQueryHandler : IRequestHandler<GetHotelRoomsQuery, Result<List<RoomResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetHotelRoomsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<RoomResponse>>> Handle(GetHotelRoomsQuery request, CancellationToken cancellationToken)
		{
			var rooms = await _unitOfWork.Rooms.GetHotelRoomsAsync(request.HotelId);
			if (rooms == null || rooms.Count == 0)
			{
				var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId, cancellationToken);
				if (hotel == null)
					return Result<List<RoomResponse>>.Failure("Hotel not found");
				return Result<List<RoomResponse>>.Success(new List<RoomResponse>());
			}

			var roomList = rooms.ToList();
			if (request.CheckIn.HasValue && request.CheckOut.HasValue && request.CheckOut > request.CheckIn)
			{
				var dateRange = new DateRange(request.CheckIn.Value, request.CheckOut.Value);
				roomList = roomList.Where(r => r.IsAvailable(dateRange)).ToList();
			}

			var dtos = roomList.Adapt<List<RoomResponse>>();
			if (request.CheckIn.HasValue && request.CheckOut.HasValue && request.CheckOut > request.CheckIn)
			{
				var dateRange = new DateRange(request.CheckIn.Value, request.CheckOut.Value);
				foreach (var dto in dtos)
				{
					var room = roomList.First(r => r.Id == dto.Id);
					dto.TotalPrice = room.CalculateTotalPrice(dateRange).Amount;
				}
			}
			return Result<List<RoomResponse>>.Success(dtos);
		}
	}
}
