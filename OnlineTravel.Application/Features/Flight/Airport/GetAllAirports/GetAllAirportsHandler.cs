using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
	public class GetAllAirportsHandler : IRequestHandler<GetAllAirportsQuery, Result<List<GetAllAirportsDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllAirportsHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<GetAllAirportsDto>>> Handle(GetAllAirportsQuery request, CancellationToken cancellationToken)
		{
			var airports = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().GetAllAsync();
			var items = airports
				.Skip(request.PageSize * (request.PageIndex - 1))
				.Take(request.PageSize)
				.Select(a => new GetAllAirportsDto
				{
					Id = a.Id,
					Name = a.Name,
					Code = a.Code.Value,
					City = a.Address.City,
					Country = a.Address.Country
				})
				.ToList();

			return Result<List<GetAllAirportsDto>>.Success(items);
		}
	}
}
