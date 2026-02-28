using Mapster;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.CarBrands.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated
{
	public class GetCarBrandsPaginatedQueryHandler : IRequestHandler<GetCarBrandsPaginatedQuery, Result<PagedResult<CarBrandResponse>>>

	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCarBrandsPaginatedQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PagedResult<CarBrandResponse>>> Handle(GetCarBrandsPaginatedQuery request, CancellationToken cancellationToken)
		{
			var repo = _unitOfWork.Repository<CarBrand>();
			var query = repo.Query();

			if (!string.IsNullOrEmpty(request.SearchTerm))
			{
				query = query.Where(b => b.Name.Contains(request.SearchTerm));
			}

			var totalCount = query.Count();
			var items = query
				.OrderByDescending(b => b.CreatedAt)
				.Skip((request.PageIndex - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToList();

			var dtos = items.Adapt<IReadOnlyList<CarBrandResponse>>();
			var result = new PagedResult<CarBrandResponse>(dtos, totalCount, request.PageIndex, request.PageSize);

			return Result<PagedResult<CarBrandResponse>>.Success(result);
		}

	}
}
