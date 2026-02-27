using MediatR;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
	public class GetAllAirportsQuery : IRequest<OnlineTravel.Domain.ErrorHandling.Result<List<GetAllAirportsDto>>>
	{
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 100;
	}
}

