namespace OnlineTravelBookingTeamB.Models
{
    public class DashboardViewModel
    {
        public int TotalTours { get; set; }
        public int TotalHotels { get; set; }
        public int TotalCars { get; set; }
        public int TotalFlights { get; set; }
        
        public decimal TotalRevenue { get; set; }
        public int ActiveBookings { get; set; }
        public int NewUsersToday { get; set; }

        public List<RecentActivityItem> RecentActivities { get; set; } = new();
    }

    public class RecentActivityItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
        public string Status { get; set; } = "info"; // success, warning, info, danger
        public string Icon { get; set; } = "bi-circle";
    }
}
