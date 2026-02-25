using System.Collections.Generic;

namespace OnlineTravelBookingTeamB.Models.Shared
{
    public class AdminPageHeaderViewModel
    {
        public string? Title { get; set; }
        public string? TitleSuffix { get; set; }
        public string? IconClass { get; set; }
        public string? IconBgClass { get; set; }
        public string? StatusText { get; set; }
        public string? StatusClass { get; set; }
        public string? StatusIconClass { get; set; }
        public string? BadgeText { get; set; }
        
        public List<BreadcrumbItem> Breadcrumbs { get; set; } = new List<BreadcrumbItem>();
        
        public bool ShowSearch { get; set; } = true;
        public string SearchInputName { get; set; } = "search";
        public string? SearchValue { get; set; }
        public string SearchPlaceholder { get; set; } = "Search...";
        
        public bool ShowStatusFilter { get; set; } = false;
        public string StatusFilterName { get; set; } = "status";
        public string StatusFilterPlaceholder { get; set; } = "All Statuses";
        public List<StatusOption>? StatusOptions { get; set; }
        
        public List<ActionButtonViewModel> ActionButtons { get; set; } = new List<ActionButtonViewModel>();
    }

    public class BreadcrumbItem
    {
        public string? Text { get; set; }
        public string? Controller { get; set; }
        public string Action { get; set; } = "Index";
        public bool IsActive { get; set; }
    }

    public class StatusOption
    {
        public string? Text { get; set; }
        public string? Value { get; set; }
        public bool Selected { get; set; }
    }

    public class ActionButtonViewModel
    {
        public string? Text { get; set; }
        public string? IconClass { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? Class { get; set; }
        public Dictionary<string, string>? RouteData { get; set; }
    }
}
