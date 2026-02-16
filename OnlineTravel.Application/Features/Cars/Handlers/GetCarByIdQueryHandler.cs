using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Application.Specifications.Carspec.OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Handlers
{
    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, Result<CarDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarDto>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new CarSpecification()
                .IncludeBrandAndCategory();

            var car = await _unitOfWork.Repository<Car>()
                .GetEntityWithAsync(spec, cancellationToken);

            if (car is null)
                return EntityError<Car>.NotFound();

            var dto = car.Adapt<CarDto>();
            return Result<CarDto>.Success(dto);
        }
    }
}
