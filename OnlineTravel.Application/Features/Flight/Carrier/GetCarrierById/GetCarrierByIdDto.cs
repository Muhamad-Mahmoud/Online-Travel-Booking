namespace OnlineTravel.Application.Features.Flight.Carrier.GetCarrierById
{
	public class GetCarrierByIdDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
		public string? Logo { get; set; }
		public bool IsActive { get; set; }
	}
}
