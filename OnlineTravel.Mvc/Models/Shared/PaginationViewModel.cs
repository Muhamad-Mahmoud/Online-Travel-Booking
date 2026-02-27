namespace OnlineTravel.Mvc.Models.Shared;

public class PaginationViewModel
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public string ActionName { get; set; } = "Index";
    public string? ControllerName { get; set; }
    public object? RouteValues { get; set; }

    public static PaginationViewModel Create<T>(OnlineTravel.Domain.Exceptions.PaginatedResult<T> result, string action, string controller, object? routeValues = null)
    {
        return new PaginationViewModel
        {
            PageIndex = result.PageIndex,
            TotalPages = result.TotalPages,
            HasPreviousPage = result.HasPrevious,
            HasNextPage = result.HasNext,
            ActionName = action,
            ControllerName = controller,
            RouteValues = routeValues
        };
    }

    public static PaginationViewModel Create<T>(OnlineTravel.Application.Common.PagedResult<T> result, string action, string controller, object? routeValues = null)
    {
        return new PaginationViewModel
        {
            PageIndex = result.PageIndex,
            TotalPages = result.TotalPages,
            HasPreviousPage = result.HasPreviousPage,
            HasNextPage = result.HasNextPage,
            ActionName = action,
            ControllerName = controller,
            RouteValues = routeValues
        };
    }
}
