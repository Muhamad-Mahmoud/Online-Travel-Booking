using AutoMapper;
using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;
using OnlineTravel.Application.Hotels.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Hotels.Public.SearchHotels
{
	public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, Result<PagedResult<HotelSearchDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SearchHotelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<PagedResult<HotelSearchDto>>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
		{
			var spec = new HotelSearchSpecification(
				city: request.City,
				minRating: request.Stars,
				pageNumber: request.PageNumber,
				pageSize: request.PageSize);

			var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(spec, cancellationToken);
			var filteredHotels = hotels.AsEnumerable();

			if (request.Latitude.HasValue && request.Longitude.HasValue && request.RadiusInKm.HasValue && request.RadiusInKm > 0)
			{
				var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
				var searchPoint = geometryFactory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value));
				var radiusInMeters = request.RadiusInKm.Value * 1000;
				filteredHotels = filteredHotels.Where(h => h.Address?.Coordinates != null && h.Address.Coordinates.Distance(searchPoint) <= radiusInMeters);
			}

			if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
			{
				filteredHotels = filteredHotels.Where(h =>
				{
					var minRoomPrice = h.Rooms.Any() ? h.Rooms.Min(r => r.BasePricePerNight.Amount) : 0m;
					return (!request.MinPrice.HasValue || minRoomPrice >= request.MinPrice.Value) &&
						   (!request.MaxPrice.HasValue || minRoomPrice <= request.MaxPrice.Value);
				});
			}

			if (request.Guests.HasValue && request.Guests.Value > 0)
			{
				filteredHotels = filteredHotels.Where(h => h.Rooms.Any(r => r.Capacity >= request.Guests.Value));
			}

			if (request.CheckIn.HasValue && request.CheckOut.HasValue && request.CheckOut > request.CheckIn)
			{
				var dateRange = new DateRange(request.CheckIn.Value, request.CheckOut.Value);
				filteredHotels = filteredHotels.Where(h => h.Rooms.Any(r => r.IsAvailable(dateRange)));
			}

			var hotelsList = filteredHotels.ToList();
			var totalCount = hotelsList.Count;
			var dtos = _mapper.Map<List<HotelSearchDto>>(hotelsList);
			var pageIndex = request.PageNumber > 0 ? request.PageNumber - 1 : 0;
			var pagedResult = new PagedResult<HotelSearchDto>(dtos, totalCount, pageIndex, request.PageSize);
			return Result<PagedResult<HotelSearchDto>>.Success(pagedResult);
		}
	}
}
