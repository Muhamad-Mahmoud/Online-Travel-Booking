using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Hotels.Admin.UpdateHotel
{
	public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result<UpdateHotelResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateHotelCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UpdateHotelResponse>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
		{
			var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(request.Id, cancellationToken);
			if (hotel == null)
				return Result<UpdateHotelResponse>.Failure("Hotel not found");

			hotel.UpdateDetails(request.Name, request.Description, request.CancellationPolicy);

			var coordinates = new GeometryFactory(new PrecisionModel(), 4326)
				.CreatePoint(new Coordinate(request.Longitude, request.Latitude));
			hotel.UpdateAddress(new Address(
				request.Street,
				request.City,
				request.State,
				request.Country,
				request.PostalCode,
				coordinates));

			hotel.UpdateContactInfo(new ContactInfo(
				!string.IsNullOrWhiteSpace(request.ContactEmail) ? new EmailAddress(request.ContactEmail) : null,
				!string.IsNullOrWhiteSpace(request.ContactPhone) ? new PhoneNumber(request.ContactPhone) : null,
				!string.IsNullOrWhiteSpace(request.Website) ? new Url(request.Website) : null));

			hotel.UpdateCheckInCheckOut(
				new TimeRange(request.CheckInTime, request.CheckInTime),
				new TimeRange(request.CheckOutTime, request.CheckOutTime));

			if (!string.IsNullOrWhiteSpace(request.MainImage))
				hotel.SetMainImage(request.MainImage);

			_unitOfWork.Repository<Hotel>().Update(hotel);
			await _unitOfWork.Complete();

			return Result<UpdateHotelResponse>.Success(new UpdateHotelResponse
			{
				Id = hotel.Id,
				Name = hotel.Name,
				UpdatedAt = hotel.UpdatedAt
			});
		}
	}
}
