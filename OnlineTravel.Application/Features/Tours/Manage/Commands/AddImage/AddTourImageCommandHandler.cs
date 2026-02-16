using MediatR;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;

public class AddTourImageCommandHandler : IRequestHandler<AddTourImageCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddTourImageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddTourImageCommand request, CancellationToken cancellationToken)
    {
        var tour = await _unitOfWork.Repository<Tour>().GetByIdAsync(request.TourId);
        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var image = new TourImage
        {
            TourId = request.TourId,
            Url = request.Url,
            AltText = request.AltText
        };

        await _unitOfWork.Repository<TourImage>().AddAsync(image, cancellationToken);
        await _unitOfWork.Complete();

        return image.Id;
    }
}
