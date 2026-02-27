namespace OnlineTravel.Mvc.Models.Shared;

public class ActionButtonViewModel
{
    public string Text { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string Action { get; set; } = "Index";
    public string? Controller { get; set; }
    public string Class { get; set; } = "btn-primary";
    public System.Collections.Generic.IDictionary<string, string>? RouteData { get; set; }
}
