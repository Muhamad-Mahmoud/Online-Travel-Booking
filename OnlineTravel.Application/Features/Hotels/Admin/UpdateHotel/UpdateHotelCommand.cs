using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel
{
	public class UpdateHotelCommand : IRequest<Result<UpdateHotelResponse>>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string MainImage { get; set; }
		public TimeOnly CheckInTime { get; set; }
		public TimeOnly CheckOutTime { get; set; }
		public string CancellationPolicy { get; set; }
		public string ContactPhone { get; set; }
		public string ContactEmail { get; set; }
		public string Website { get; set; }
	}

}
