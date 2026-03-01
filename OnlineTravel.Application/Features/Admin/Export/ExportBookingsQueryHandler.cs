using System.Text;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.Helpers;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Admin.Export;

public class ExportBookingsQueryHandler : IRequestHandler<ExportBookingsQuery, Result<byte[]>>
{
	private readonly IUnitOfWork _unitOfWork;

	// Batch size — keeps memory usage bounded to ~1 000 records at a time
	private const int BatchSize = 1_000;

	public ExportBookingsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<byte[]>> Handle(ExportBookingsQuery request, CancellationToken cancellationToken)
	{
		var csv = new StringBuilder();
		csv.AppendLine("Reference,User,Service,Booked On,Start Date,Amount,Currency,Status,PaymentStatus");

		int pageIndex = 1;
		bool hasMore = true;

		while (hasMore)
		{
			var spec = new GetAllBookingsSpec(pageIndex, BatchSize, null, null);
			var batch = await _unitOfWork.Repository<BookingEntity>()
				.GetAllWithSpecAsync(spec, cancellationToken);

			if (batch.Count == 0)
				break;

			// Handle lazy expiration for this batch
			if (BookingExpirationHelper.MarkExpiredBookings(batch))
			{
				await _unitOfWork.Complete();
			}

			foreach (var b in batch)
			{
				var userName = b.User?.Name ?? "Unknown";
				var detail = b.Details.FirstOrDefault();
				var itemName = detail?.ItemName ?? "N/A";
				var startDate = detail?.StayRange.Start.ToString("yyyy-MM-dd") ?? "N/A";

				csv.AppendLine(
					$"{b.BookingReference.Value}," +
					$"{EscapeCsv(userName)}," +
					$"{EscapeCsv(itemName)}," +
					$"{b.BookingDate:yyyy-MM-dd HH:mm}," +
					$"{startDate}," +
					$"{b.TotalPrice.Amount}," +
					$"{b.TotalPrice.Currency}," +
					$"{b.Status}," +
					$"{b.PaymentStatus}");
			}

			// If fewer rows were returned than the batch size, this was the last page
			hasMore = batch.Count == BatchSize;
			pageIndex++;
		}

		return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
	}

	private static string EscapeCsv(string field)
	{
		if (string.IsNullOrEmpty(field)) return "";
		if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
			return $"\"{field.Replace("\"", "\"\"")}\"";
		return field;
	}
}

