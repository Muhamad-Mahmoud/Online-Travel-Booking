using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Hotels.Public.SearchHotels
{
	public class SearchHotelsQuery : IRequest<Result<PagedResult<HotelSearchResponse>>>
	{
		public string City { get; set; }
		public DateOnly? CheckIn { get; set; }
		public DateOnly? CheckOut { get; set; }
		public int? Guests { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
		public int? Stars { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public double? RadiusInKm { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;

	}
}
