using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.EditRoom
{
    public class EditRoomCommandHandler : IRequestHandler<EditRoomCommand, Result<EditRoomResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EditRoomResponse>> Handle(EditRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(request.Id, cancellationToken);
            if (room == null)
                return Result<EditRoomResponse>.Failure("Room not found");

            room.UpdateDetails(request.Name, request.Description, new Money(request.BasePricePerNight, "USD"));
            _unitOfWork.Repository<Room>().Update(room);
            await _unitOfWork.Complete();

            return Result<EditRoomResponse>.Success(new EditRoomResponse
            {
                Id = room.Id,
                Name = room.Name,
                UpdatedAt = room.UpdatedAt
            });
        }
    }
}
