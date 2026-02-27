using System.Collections.Generic;

namespace OnlineTravel.Mvc.Models.Shared;

public class AdminPageHeaderViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? TitleSuffix { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public string? IconBgClass { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string? StatusIconClass { get; set; }
    public string? StatusClass { get; set; }
    public string? BadgeText { get; set; }
    public List<BreadcrumbItem> Breadcrumbs { get; set; } = new();
    public bool ShowSearch { get; set; }
    public string SearchInputName { get; set; } = "search";
    public string? SearchValue { get; set; }
    public string SearchPlaceholder { get; set; } = "Search...";
    public bool ShowStatusFilter { get; set; }
    public string StatusFilterName { get; set; } = "status";
    public string StatusFilterPlaceholder { get; set; } = "All";
    public List<StatusOption>? StatusOptions { get; set; }
    public List<ActionButtonViewModel>? ActionButtons { get; set; }
}
