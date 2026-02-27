using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand
{
	public class CreateHotelCommand : IRequest<Result<CreateHotelResponse>>
	{
		public string Name { get; init; } = string.Empty;
		public string Slug { get; init; } = string.Empty;
		public string Description { get; init; } = string.Empty;

		public string Street { get; init; } = string.Empty;
		public string City { get; init; } = string.Empty;
		public string State { get; init; } = string.Empty;
		public string Country { get; init; } = string.Empty;
		public string PostalCode { get; init; } = string.Empty;
		public double? Latitude { get; init; }
		public double? Longitude { get; init; }

		public TimeOnly CheckInTimeStart { get; init; }
		public TimeOnly CheckInTimeEnd { get; init; }
		public TimeOnly CheckOutTimeStart { get; init; }
		public TimeOnly CheckOutTimeEnd { get; init; }

		public string CancellationPolicy { get; init; } = string.Empty;
		public string ContactPhone { get; init; } = string.Empty;
		public string ContactEmail { get; init; } = string.Empty;
		public string Website { get; init; } = string.Empty;
		public string MainImage { get; set; } = string.Empty;
	}
}
