using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated
{
	public class GetCarBrandsPaginatedQueryHandler : IRequestHandler<GetCarBrandsPaginatedQuery, Result<PaginatedResult<CarBrandDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCarBrandsPaginatedQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedResult<CarBrandDto>>> Handle(GetCarBrandsPaginatedQuery request, CancellationToken cancellationToken)
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

			var dtos = items.Adapt<IReadOnlyList<CarBrandDto>>();
			var result = new PaginatedResult<CarBrandDto>(request.PageIndex, request.PageSize, totalCount, dtos);

			return Result<PaginatedResult<CarBrandDto>>.Success(result);
		}
	}
}
