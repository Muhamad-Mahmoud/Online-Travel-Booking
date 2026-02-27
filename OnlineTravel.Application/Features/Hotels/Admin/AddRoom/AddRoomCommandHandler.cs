using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.AddRoom
{
	public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Result<AddRoomResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddRoomCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AddRoomResponse>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
		{
			var hotel = await _unitOfWork.Hotels.GetWithRoomsAsync(request.HotelId);
			if (hotel == null)
				return Result<AddRoomResponse>.Failure("Hotel not found");

			var basePrice = new Money(request.BasePricePerNight, "USD");
			var room = new Room(
				request.HotelId,
				request.RoomNumber.Trim(),
				request.Name,
				request.Description,
				basePrice);

			if (request.Photos != null)
			{
				foreach (var photo in request.Photos)
				{
					if (!string.IsNullOrWhiteSpace(photo.Url))
						room.AddPhoto(new Url(photo.Url));
				}
			}

			hotel.AddRoom(room);
			await _unitOfWork.Repository<Room>().AddAsync(room, cancellationToken);
			await _unitOfWork.Complete();

			return Result<AddRoomResponse>.Success(new AddRoomResponse
			{
				Id = room.Id,
				HotelId = hotel.Id,
				Name = room.Name
			});
		}
	}
}
