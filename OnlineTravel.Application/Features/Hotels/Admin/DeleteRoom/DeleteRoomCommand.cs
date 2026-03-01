using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom;

public class DeleteRoomCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
