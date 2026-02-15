using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Handlers
{
    public class GetCarDetailsByIdQueryHandler : IRequestHandler<GetCarDetailsByIdQuery, Result<CarDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarDetailsByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarDetailsDto>> Handle(GetCarDetailsByIdQuery request, CancellationToken cancellationToken)
        {
      
            var spec = new CarDetailsSpecification()
                .IncludeAllDetails();

            // فلترة بالـ Id
            spec.Criteria = x => x.Id == request.Id;

            var car = await _unitOfWork.Repository<Car>().GetEntityWithAsync(spec, cancellationToken);
            if (car is null)
                return Result<CarDetailsDto>.Failure(EntityError<Car>.NotFound());

            var dto = car.Adapt<CarDetailsDto>();
            return Result<CarDetailsDto>.Success(dto);
        }
    }


    public class CarDetailsSpecification : BaseSpecification<Car>
    {
        public CarDetailsSpecification IncludeAllDetails()
        {
            // هنا AddIncludes ممكن تستخدمه لأنه داخل الـ subclass
            AddIncludes(x => x.Brand);
            AddIncludes(x => x.Category);
            AddIncludes(x => x.PricingTiers);
   
            return this;
        }
    }

}
