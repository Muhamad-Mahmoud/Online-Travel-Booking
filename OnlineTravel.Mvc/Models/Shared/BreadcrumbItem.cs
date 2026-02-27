namespace OnlineTravel.Mvc.Models.Shared;

public class BreadcrumbItem
{
    public string Text { get; set; } = string.Empty;
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public bool IsActive { get; set; }
}
