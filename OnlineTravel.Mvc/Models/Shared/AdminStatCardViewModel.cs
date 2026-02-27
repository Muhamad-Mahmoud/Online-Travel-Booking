namespace OnlineTravel.Mvc.Models.Shared;

public class AdminStatCardViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string? IconBgClass { get; set; }
    public string? IconTextClass { get; set; }
    public string? TrendText { get; set; }
    public string? TrendClass { get; set; }
    public string? Subtitle { get; set; }
    public string? SubtitleClass { get; set; }
}
