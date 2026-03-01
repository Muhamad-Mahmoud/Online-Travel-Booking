using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.CreateHotel
{
	public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<CreateHotelResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateHotelCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CreateHotelResponse>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
		{
			var hotel = Hotel.Create(
				request.Name,
				request.Slug,
				request.Description,
				request.Street,
				request.City,
				request.State,
				request.Country,
				request.PostalCode,
				request.Latitude,
				request.Longitude,
				request.CheckInTimeStart,
				request.CheckInTimeEnd,
				request.CheckOutTimeStart,
				request.CheckOutTimeEnd,
				request.CancellationPolicy,
				request.ContactPhone,
				request.ContactEmail,
				request.Website,
				request.MainImage
			);

			await _unitOfWork.Hotels.AddAsync(hotel);
			await _unitOfWork.Complete();

			var response = new CreateHotelResponse
			{
				Id = hotel.Id,
				Name = hotel.Name,
				Slug = hotel.Slug
			};
			return Result<CreateHotelResponse>.Success(response);
		}
	}
}
