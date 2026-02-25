using Mapster;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravelBookingTeamB.Models
{
    public class CarIndexViewModel
    {
        public PaginatedResult<CarSummaryDto>? Cars { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
        public OnlineTravel.Domain.Enums.CarCategory? CarType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // للعرض: مجموع الصفحات
        public int TotalPages => Cars == null ? 0 : (int)Math.Ceiling(Cars.TotalCount / (double)PageSize);
    }
}
