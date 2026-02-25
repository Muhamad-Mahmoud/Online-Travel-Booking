using Microsoft.AspNetCore.Routing;

namespace OnlineTravelBookingTeamB.Models
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string ActionName { get; set; } = "Index";
        public string? ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; } = new();

        public static PaginationViewModel Create(dynamic pagedResult, string actionName, string? controllerName, object? additionalRouteValues = null)
        {
            if (pagedResult == null) return new PaginationViewModel();

            var vm = new PaginationViewModel
            {
                PageIndex = pagedResult.PageIndex,
                TotalPages = pagedResult.TotalPages,
                ActionName = actionName,
                ControllerName = controllerName
            };

            // Handle both PaginatedResult and PagedResult naming conventions
            try { vm.HasPreviousPage = pagedResult.HasPrevious; } catch { vm.HasPreviousPage = pagedResult.HasPreviousPage; }
            try { vm.HasNextPage = pagedResult.HasNext; } catch { vm.HasNextPage = pagedResult.HasNextPage; }

            if (additionalRouteValues != null)
            {
                vm.RouteValues = new RouteValueDictionary(additionalRouteValues);
            }

            return vm;
        }
    }
}
