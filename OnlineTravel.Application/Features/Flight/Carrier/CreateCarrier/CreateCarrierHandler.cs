using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier
{
    public class CreateCarrierHandler : IRequestHandler<CreateCarrierCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarrierHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCarrierCommand request, CancellationToken cancellationToken)
        {
            // 1. Map to Entity using the full path to avoid conflicts
            var carrier = new OnlineTravel.Domain.Entities.Flights.Carrier
            {
                Name = request.Name,
                Code = new IataCode(request.Code),
                Logo = request.Logo,
                IsActive = true
                // Note: Add ContactInfo mapping here based on your Value Object structure
            };

            // 2. Add and Save
            await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>().AddAsync(carrier);
            await _unitOfWork.Complete();

            return carrier.Id;
        }
    }
}
