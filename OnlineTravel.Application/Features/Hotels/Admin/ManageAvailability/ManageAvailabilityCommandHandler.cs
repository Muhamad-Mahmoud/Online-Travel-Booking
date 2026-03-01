using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability
{
	public class ManageAvailabilityCommandHandler : IRequestHandler<ManageAvailabilityCommand, Result<ManageAvailabilityResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public ManageAvailabilityCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<ManageAvailabilityResponse>> Handle(ManageAvailabilityCommand request, CancellationToken cancellationToken)
		{
			var room = await _unitOfWork.Rooms.GetWithAvailabilityAsync(request.RoomId);
			if (room == null)
				return Result<ManageAvailabilityResponse>.Failure("Room not found");

			var dateRange = new DateRange(request.StartDate, request.EndDate);
			var toRemove = room.RoomAvailabilities
				.Where(a => a.DateRange.OverlapsWith(dateRange))
				.ToList();

			var availability = new RoomAvailability(room.Id, dateRange, request.IsAvailable);
			room.SetAvailability(availability);

			foreach (var existing in toRemove)
				_unitOfWork.Repository<RoomAvailability>().Delete(existing);
			await _unitOfWork.Repository<RoomAvailability>().AddAsync(availability, cancellationToken);
			await _unitOfWork.Complete();

			return Result<ManageAvailabilityResponse>.Success(new ManageAvailabilityResponse
			{
				RoomId = room.Id,
				Message = $"Availability updated for period {request.StartDate:yyyy-MM-dd} to {request.EndDate:yyyy-MM-dd}"
			});
		}
	}
}
