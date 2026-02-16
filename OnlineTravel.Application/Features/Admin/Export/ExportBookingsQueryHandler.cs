using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using System.Text;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Helpers;

namespace OnlineTravel.Application.Features.Admin.Export;

public class ExportBookingsQueryHandler : IRequestHandler<ExportBookingsQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportBookingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<byte[]>> Handle(ExportBookingsQuery request, CancellationToken cancellationToken)
    {
        // Fetch all bookings
        var spec = new GetAllBookingsSpec(1, int.MaxValue, null, null); 
        var bookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(spec, cancellationToken);

        // Handle lazy expiration (consistent with other handlers)
        if (BookingExpirationHelper.MarkExpiredBookings(bookings))
        {
            await _unitOfWork.Complete();
        }

        // Generate CSV
        var csv = new StringBuilder();
        csv.AppendLine("Reference,User,Service,Booked On,Start Date,Amount,Currency,Status,PaymentStatus");

        foreach (var b in bookings)
        {
            var userName = b.User?.Name ?? "Unknown";
            var detail = b.Details.FirstOrDefault();
            var itemName = detail?.ItemName ?? "N/A";
            var startDate = detail?.StayRange.Start.ToString("yyyy-MM-dd") ?? "N/A";
            
            csv.AppendLine($"{b.BookingReference.Value},{EscapeCsv(userName)},{EscapeCsv(itemName)},{b.BookingDate:yyyy-MM-dd HH:mm},{startDate},{b.TotalPrice.Amount},{b.TotalPrice.Currency},{b.Status},{b.PaymentStatus}");
        }

        return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
    }

    private static string EscapeCsv(string field)
    {
        if (string.IsNullOrEmpty(field)) return "";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}
