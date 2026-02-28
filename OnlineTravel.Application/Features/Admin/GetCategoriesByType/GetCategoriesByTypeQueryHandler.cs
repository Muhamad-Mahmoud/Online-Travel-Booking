using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Admin.GetCategoriesByType
{
	public class GetCategoriesByTypeQueryHandler : IRequestHandler<GetCategoriesByTypeQuery, Result<List<CategoryResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCategoriesByTypeQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesByTypeQuery request, CancellationToken cancellationToken)
		{
			var categories = await _unitOfWork.Repository<Category>()
				.GetAllAsync(cancellationToken);

			var filtered = categories.Where(c => c.Type == request.Type).ToList();
			var dtos = filtered.Adapt<List<CategoryResponse>>();

			return Result<List<CategoryResponse>>.Success(dtos);
		}
	}
}
