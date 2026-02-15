using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Commands
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var car = request.Adapt<Car>();
            car.Id = Guid.NewGuid();
            car.CreatedAt = DateTime.UtcNow;
            car.UpdatedAt = DateTime.UtcNow;

            // PricingTiers
            car.PricingTiers = request.PricingTiers
                .Select(t => new CarPricingTier
                {
                    Id = Guid.NewGuid(),
                    CarId = car.Id,
                    FromHours = t.FromHours,
                    ToHours = t.ToHours,
                    PricePerHour = t.PricePerHour,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                })
                .ToList();

            car.Images = request.Images
                .Select(img => new ImageUrl(img.Url, img.AltText))
                .ToList();

            var repo = _unitOfWork.Repository<Car>();
            await repo.AddAsync(car);
            await _unitOfWork.Complete();

            return car.Id;
        }
    }
}
