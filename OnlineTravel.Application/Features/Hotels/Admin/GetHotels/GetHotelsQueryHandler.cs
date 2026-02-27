using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Hotels.Admin.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, Result<PaginatedResult<Hotel>>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetHotelsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<PaginatedResult<Hotel>>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
	{
		var repository = _unitOfWork.Repository<Hotel>();
		var query = repository.Query()
			.Include(h => h.Rooms)
			.Include(h => h.Address)
			.Include(h => h.Reviews)
			.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			query = query.Where(h => h.Name.Contains(request.Search) || h.Slug.Contains(request.Search));
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var items = await query
			.OrderBy(h => h.Name)
			.Skip((request.PageIndex - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var result = new PaginatedResult<Hotel>(request.PageIndex, request.PageSize, totalCount, items);
		return Result<PaginatedResult<Hotel>>.Success(result);
	}
}
