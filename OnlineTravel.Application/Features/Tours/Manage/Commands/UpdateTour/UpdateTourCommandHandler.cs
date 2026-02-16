using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour;

public class UpdateTourCommandHandler : IRequestHandler<UpdateTourCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
        if (tour == null) return false;

        tour.Title = request.Title;
        tour.Description = request.Description;
        tour.CategoryId = request.CategoryId;
        tour.DurationDays = request.DurationDays;
        tour.DurationNights = request.DurationNights;
        tour.Recommended = request.Recommended;
        tour.BestTimeToVisit = request.BestTimeToVisit;

        tour.UpdateAddress(new Address(
            request.Street, request.City, request.State, request.Country, request.PostalCode,
            tour.Address?.Coordinates // preserve existing coordinates
        ));

        if (!string.IsNullOrEmpty(request.MainImageUrl))
        {
            tour.MainImage = new ImageUrl(request.MainImageUrl);
        }

        _unitOfWork.Repository<Tour>().Update(tour);
        await _unitOfWork.Complete();

        return true;
    }
}
