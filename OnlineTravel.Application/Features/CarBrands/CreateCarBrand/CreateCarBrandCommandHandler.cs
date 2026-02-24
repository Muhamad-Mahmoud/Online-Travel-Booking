using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.CreateCarBrand
{
    public class CreateCarBrandCommandHandler : IRequestHandler<CreateCarBrandCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCarBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = request.Data.Adapt<CarBrand>();

            await _unitOfWork.Repository<CarBrand>().AddAsync(brand, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
                return Result<Guid>.Success(brand.Id);

            return Result<Guid>.Failure(new Error("CarBrand.Create", "Failed to create car brand.", 500));
        }
    }
}
