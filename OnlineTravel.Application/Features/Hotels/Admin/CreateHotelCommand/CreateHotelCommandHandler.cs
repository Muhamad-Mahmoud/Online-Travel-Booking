using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand
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
            Point? coordinates = request.Latitude.HasValue && request.Longitude.HasValue
                ? new Point(request.Longitude.Value, request.Latitude.Value) { SRID = 4326 }
                : null;

            var address = new Address(
                request.Street,
                request.City,
                request.State,
                request.Country,
                request.PostalCode,
                coordinates
            );

            PhoneNumber? phone = !string.IsNullOrWhiteSpace(request.ContactPhone)
                ? new PhoneNumber(request.ContactPhone)
                : null;
            EmailAddress? email = !string.IsNullOrWhiteSpace(request.ContactEmail)
                ? new EmailAddress(request.ContactEmail)
                : null;
            Url? website = !string.IsNullOrWhiteSpace(request.Website)
                ? new Url(request.Website)
                : null;
            var contactInfo = new ContactInfo(email, phone, website);

            var checkInTime = new TimeRange(request.CheckInTimeStart, request.CheckInTimeEnd);
            var checkOutTime = new TimeRange(request.CheckOutTimeStart, request.CheckOutTimeEnd);

            var hotel = new Hotel(
                request.Name,
                request.Slug,
                request.Description,
                address,
                contactInfo,
                checkInTime,
                checkOutTime,
                request.CancellationPolicy,
                request.MainImageUrl
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
