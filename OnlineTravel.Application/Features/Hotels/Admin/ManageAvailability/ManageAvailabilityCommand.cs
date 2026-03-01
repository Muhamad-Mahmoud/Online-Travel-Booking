using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability
{
	public class ManageAvailabilityCommand : IRequest<Result<ManageAvailabilityResponse>>
	{
		public Guid RoomId { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public bool IsAvailable { get; set; }

	}
}
