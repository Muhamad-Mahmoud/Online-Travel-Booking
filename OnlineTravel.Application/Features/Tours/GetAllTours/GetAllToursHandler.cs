using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Tours.Specifications;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Domain.ErrorHandling;


namespace OnlineTravel.Application.Features.Tours.GetAllTours;

public class GetAllToursHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllToursQuery, OnlineTravel.Application.Common.Result<PagedResult<TourResponse>>>
{


	public async Task<OnlineTravel.Application.Common.Result<PagedResult<TourResponse>>> Handle(GetAllToursQuery request, CancellationToken cancellationToken)

	{
		var countSpec = new AllToursWithPricingSpecification(request.Search, request.Lat, request.Lon, request.RadiusKm, request.MinPrice, request.MaxPrice, request.Rating, request.City, request.Country, request.SortOrder);
		var totalCount = await unitOfWork.Repository<Tour>().GetCountAsync(countSpec, cancellationToken);

		var dataSpec = new AllToursWithPricingSpecification(request.Search, request.Lat, request.Lon, request.RadiusKm, request.MinPrice, request.MaxPrice, request.Rating, request.City, request.Country, request.SortOrder);
		dataSpec.ApplyPagination(request.PageSize * (request.PageIndex - 1), request.PageSize);
		var tours = await unitOfWork.Repository<Tour>().GetAllWithSpecAsync(dataSpec, cancellationToken);


		var data = tours.Select(tour =>
		{
			var lowestPrice = tour.PriceTiers
				.OrderBy(p => p.Price.Amount)
				.FirstOrDefault()?.Price;

			return new TourResponse
			{
				Id = tour.Id,
				Title = tour.Title,
				Description = tour.Description ?? string.Empty,
				City = tour.Address?.City ?? string.Empty,
				Country = tour.Address?.Country ?? string.Empty,
				ImageUrl = tour.MainImage?.Url ?? string.Empty,
				Category = tour.Category?.Title ?? string.Empty,
				Rating = tour.Reviews.Any() ? (double)tour.Reviews.Average(r => r.Rating.Value) : 0,
				Price = lowestPrice?.Amount ?? 0,
				Currency = lowestPrice?.Currency ?? "USD",
				IsFavorite = false // Placeholder until favorites feature is implemented
			};
		}).ToList();

		var result = new PagedResult<TourResponse>(data, totalCount, request.PageIndex, request.PageSize);
		return OnlineTravel.Application.Common.Result<PagedResult<TourResponse>>.Success(result);
	}
}

