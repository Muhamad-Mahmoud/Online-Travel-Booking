using MediatR;
using OnlineTravel.Application.Features.Tours.CreateTour.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Tours.CreateTour.Handlers
{
    public class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTourCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateTourCommand request, CancellationToken cancellationToken)
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
                Address = new Address(request.Street, request.City, request.State, request.Country, request.PostalCode),
                MainImage = !string.IsNullOrEmpty(request.MainImageUrl) ? new ImageUrl(request.MainImageUrl) : null
            };

            // Add Price Tier
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
            await _unitOfWork.Complete();

            return tour.Id;
        }
    }
}
