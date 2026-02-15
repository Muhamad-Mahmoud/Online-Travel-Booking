using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Carrier.GetCarrierById
{
    public class GetCarrierByIdHandler : IRequestHandler<GetCarrierByIdQuery, GetCarrierByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarrierByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCarrierByIdDto?> Handle(GetCarrierByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch carrier from repository
            var carrier = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().GetByIdAsync(request.Id);

            if (carrier == null) return null;

            // 2. Map Entity to DTO
            return new GetCarrierByIdDto
            {
                Id = carrier.Id,
                Name = carrier.Name,
                Code = carrier.Code.Value, // Accessing Value from IataCode Value Object
                Logo = carrier.Logo,
                IsActive = carrier.IsActive
            };
        }
    }
}
