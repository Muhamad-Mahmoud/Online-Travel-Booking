using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Queries

{
	public class GetAllBookingsSpec : BaseSpecification<BookingEntity>
	{
		public GetAllBookingsSpec(int pageIndex, int pageSize, string? searchTerm, string? status, bool isCount = false)
			: base(b =>
				(string.IsNullOrEmpty(searchTerm) ||
				 b.BookingReference.Value.Contains(searchTerm) ||
				 b.User.Name.Contains(searchTerm) ||
				 b.Details.Any(d => d.ItemName.Contains(searchTerm) || d.Category.Title.Contains(searchTerm))) &&
				(string.IsNullOrEmpty(status) ||
					(status == "PendingPayment" && b.Status == OnlineTravel.Domain.Enums.BookingStatus.PendingPayment && b.ExpiresAt > DateTime.UtcNow) ||
					(status == "Expired" && (b.Status == OnlineTravel.Domain.Enums.BookingStatus.Expired || (b.Status == OnlineTravel.Domain.Enums.BookingStatus.PendingPayment && b.ExpiresAt <= DateTime.UtcNow))) ||
					(status != "PendingPayment" && status != "Expired" && b.Status.ToString() == status)
				)
			)
		{
			if (!isCount)
			{
				AddIncludes(b => b.Details);
				AddIncludes(b => b.Details.Select(d => d.Category));
				AddIncludes(b => b.User);

				if (pageSize != int.MaxValue)
				{
					ApplyPagination(pageSize * (pageIndex - 1), pageSize);
				}
				AddOrderByDesc(b => b.BookingDate);
			}
		}
	}
}
