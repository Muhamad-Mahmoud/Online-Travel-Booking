using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Repository<Room>().GetByIdAsync(request.Id);
        if (room == null) return Result<bool>.Failure("Room not found");

        _unitOfWork.Repository<Room>().Delete(room);
        var affected = await _unitOfWork.Complete();
        return Result<bool>.Success(affected > 0);
    }
}
