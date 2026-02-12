using MediatR;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;

namespace OnlineTravel.Application.Features.Hotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, CreateHotelResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateHotelResponse> Handle(
            CreateHotelCommand request,
            CancellationToken cancellationToken)
        {
            // STEP 1: Create Address with coordinates (if provided)
            Point? coordinates = null;
            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                
                var geometryFactory = NtsGeometryServices.Instance
                    .CreateGeometryFactory(srid: 4326);

                // Note: Point constructor takes (X, Y) = (Longitude, Latitude)
                coordinates = geometryFactory.CreatePoint(
                    new Coordinate(request.Longitude.Value, request.Latitude.Value));
            }

            var address = new Address(
                street: request.Street,
                city: request.City,
                state: request.State,
                country: request.Country,
                postalCode: request.PostalCode,
                coordinates: coordinates);

            // STEP 2: Create ContactInfo (if provided)
            ContactInfo? contactInfo = null;
            if (!string.IsNullOrWhiteSpace(request.Email) ||
                !string.IsNullOrWhiteSpace(request.Phone) ||
                !string.IsNullOrWhiteSpace(request.Website))
            {
                contactInfo = new ContactInfo(
                    email: !string.IsNullOrWhiteSpace(request.Email) ? new EmailAddress(request.Email) : null,
                    phone: !string.IsNullOrWhiteSpace(request.Phone) ? new PhoneNumber(request.Phone) : null,
                    website: !string.IsNullOrWhiteSpace(request.Website) ? new Url(request.Website) : null);
            }

            // STEP 3: Create MainImage
            var mainImage = new ImageUrl(request.MainImageUrl, request.MainImageAlt);

            // STEP 4: Create Gallery images
            var gallery = request.GalleryUrls
                .Select(url => new ImageUrl(url))
                .ToList();

            // STEP 5: Create StarRating (if provided)
            StarRating? starRating = null;
            if (request.StarRating.HasValue)
            {
                starRating = new StarRating(request.StarRating.Value);
            }

            // STEP 6: Create Hotel entity
            var hotel = new OnlineTravel.Domain.Entities.Hotels.Hotel
            {
                Name = request.Name,
                Description = request.Description,
                Address = address,
                ContactInfo = contactInfo,
                MainImage = mainImage,
                Gallery = gallery,
                StarRating = starRating,
                CategoryId = request.CategoryId
            };

            // STEP 7: Save to database
            await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Hotels.Hotel>()
                .AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();

            // STEP 8: Return response
            return new CreateHotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name,
                City = hotel.Address.City ?? string.Empty,
                Country = hotel.Address.Country ?? string.Empty,
                MainImageUrl = hotel.MainImage?.Url ?? string.Empty,
                StarRating = hotel.StarRating?.Value,
                CreatedAt = hotel.CreatedAt
            };
        }
    }
}
