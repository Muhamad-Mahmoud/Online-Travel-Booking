using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Hotels.Admin.EditRoom
{
	public class EditRoomCommand : IRequest<Result<EditRoomResponse>>
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal BasePricePerNight { get; set; }
	}

}
