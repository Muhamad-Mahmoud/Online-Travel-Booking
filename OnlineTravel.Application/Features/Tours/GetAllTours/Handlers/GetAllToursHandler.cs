using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.GetAllTours.Handlers;

public class GetAllToursHandler : IRequestHandler<GetAllToursQuery, PagedResult<TourResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllToursHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<TourResponse>> Handle(GetAllToursQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new AllToursWithPricingSpecification();
        var totalCount = await _unitOfWork.Repository<Tour>().GetCountAsync(countSpec);

        var dataSpec = new AllToursWithPricingSpecification();
        dataSpec.ApplyPagination(request.PageSize * (request.PageIndex - 1), request.PageSize);
        var tours = await _unitOfWork.Repository<Tour>().GetAllWithSpecAsync(dataSpec);

        var data = tours.Select(tour =>
        {
            var lowestPrice = tour.PriceTiers
                .OrderBy(p => p.Price.Amount)
                .FirstOrDefault()?.Price;

            return new TourResponse
            {
                Id = tour.Id,
                Title = tour.Title,
                ImageUrl = tour.MainImage?.Url ?? string.Empty,
                Category = tour.Category?.Title ?? string.Empty,
                Rating = 4.5, // Placeholder until reviews are seeded
                Price = lowestPrice?.Amount ?? 0,
                Currency = lowestPrice?.Currency ?? "USD",
                IsFavorite = false // Placeholder until favorites feature is implemented
            };
        }).ToList();

        return new PagedResult<TourResponse>(data, totalCount, request.PageIndex, request.PageSize);
    }
}
