using MediatR;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity;

public class AddTourActivityCommandHandler : IRequestHandler<AddTourActivityCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddTourActivityCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddTourActivityCommand request, CancellationToken cancellationToken)
    {
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var activity = new TourActivity
        {
            TourId = request.TourId,
            Title = request.Title,
            Description = request.Description,
            Image = new ImageUrl(request.ImageUrl)
        };

        await _unitOfWork.Repository<TourActivity>().AddAsync(activity, cancellationToken);
        await _unitOfWork.Complete();

        return activity.Id;
    }
}
