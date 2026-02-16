using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.Cars.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Handlers
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Car>();

            var car = await repo.GetByIdAsync(request.Id, cancellationToken);
            if (car is null)
                return Result.Failure(EntityError<Car>.NotFound());

            if (car.DeletedAt != null)
                return Result.Success();

            car.DeletedAt = DateTime.UtcNow;

            repo.MarkPropertyModified(car, x => x.DeletedAt); // ✅ الحل الصحيح

            var affected = await _unitOfWork.Complete();

            return affected > 0
                ? Result.Success()
                : Result.Failure(EntityError<Car>.OperationFailed("Delete failed"));
        }


    }
}
