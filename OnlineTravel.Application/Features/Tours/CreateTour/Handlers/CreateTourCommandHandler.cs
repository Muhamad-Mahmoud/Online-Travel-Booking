using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Features.Tours.CreateTour.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Tours.CreateTour.Handlers
{
    public class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTourCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateTourCommand request, CancellationToken cancellationToken)
        {
            var tour = new Tour
            {
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                DurationDays = request.DurationDays,
                DurationNights = request.DurationNights,
                Recommended = request.Recommended,
                BestTimeToVisit = request.BestTimeToVisit,
                Address = new Address(
                    request.Street,
                    request.City,
                    request.State,
                    request.Country,
                    request.PostalCode,
                    (request.Latitude.HasValue && request.Longitude.HasValue)
                        ? new Point(request.Longitude.Value, request.Latitude.Value) { SRID = 4326 }
                        : null),
                MainImage = !string.IsNullOrEmpty(request.MainImageUrl) ? new ImageUrl(request.MainImageUrl) : null
            };

            if (request.StandardPrice > 0)
            {
                tour.PriceTiers.Add(new TourPriceTier
                {
                    Name = "Standard",
                    Price = new Money(request.StandardPrice, request.Currency),
                    Description = "Standard price per person"
                });
            }

            await _unitOfWork.Repository<Tour>().AddAsync(tour, cancellationToken);
            var affectedRows = await _unitOfWork.Complete();
            if (affectedRows <= 0)
            {
                return Result<Guid>.Failure(Error.InternalServer("Failed to create tour."));
            }

            return Result<Guid>.Success(tour.Id);
        }
    }
}
