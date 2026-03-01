using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Features.Hotels.Public.SearchHotels
{
	public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, Result<PagedResult<HotelSearchResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public SearchHotelsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PagedResult<HotelSearchResponse>>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
		{
			var hotels = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Hotels.Hotel>().GetAllAsync();
			var data = hotels.Select(h => new HotelSearchResponse { Id = h.Id, Name = h.Name }).ToList();
			return Result<PagedResult<HotelSearchResponse>>.Success(new PagedResult<HotelSearchResponse>(data, hotels.Count, request.PageNumber, request.PageSize));
		}
	}
}

