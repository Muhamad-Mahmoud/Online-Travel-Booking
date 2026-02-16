using MediatR;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Application.Features.Tours.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;

namespace OnlineTravel.Application.Features.Tours.GetTourById.Handlers;

public class GetTourByIdHandler : IRequestHandler<GetTourByIdQuery, TourDetailsResponse?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTourByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TourDetailsResponse?> Handle(GetTourByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new TourWithDetailsSpecification(request.Id);
        var tour = await _unitOfWork.Repository<Tour>().GetEntityWithAsync(spec);

        if (tour == null) return null;

        var lowestPrice = tour.PriceTiers.OrderBy(p => p.Price.Amount).FirstOrDefault()?.Price;

        return new TourDetailsResponse
        {
            Id = tour.Id,
            Title = tour.Title,
            Category = tour.Category.Title,
            Location = tour.Address,
            DurationDays = tour.DurationDays,
            DurationNights = tour.DurationNights,
            Rating = tour.Reviews.Any() ? (double)tour.Reviews.Average(r => r.Rating.Value) : 0,
            ReviewCount = tour.Reviews.Count,
            MainImageUrl = tour.MainImage?.Url ?? string.Empty,
            Description = tour.Description ?? string.Empty,
            Activities = tour.Activities.Select(a => new TourActivityDto
            {
                Title = a.Title,
                Description = a.Description,
                ImageUrl = a.Image.Url
            }).ToList(),
            BestTimeToVisit = tour.BestTimeToVisit ?? "Year-round",
            Images = tour.Images.Select(i => new TourImageDto { Id = i.Id, Url = i.Url, AltText = i.AltText }).ToList(),
            PriceTiers = tour.PriceTiers.Select(p => new TourPriceTierDto { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description }).ToList(),
            Price = lowestPrice != null ? new PriceDto { Amount = lowestPrice.Amount, Currency = lowestPrice.Currency } : null
        };
    }
}
