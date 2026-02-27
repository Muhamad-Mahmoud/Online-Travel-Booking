using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom;

public class DeleteRoomCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
