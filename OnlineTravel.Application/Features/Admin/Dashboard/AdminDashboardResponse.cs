using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Admin.Dashboard;

public sealed record AdminDashboardResponse
{
    public decimal TotalRevenue { get; init; }
    public int NewBookingsCount { get; init; }
    public int ActiveToursCount { get; init; }
    public int PendingRequestsCount { get; init; }
    
    public List<MonthlyRevenueDto> RevenueAnalytics { get; init; } = new();
    public List<CategoryDistributionDto> CategoryDistribution { get; init; } = new();
    public List<RecentBookingDto> RecentBookings { get; init; } = new();
}

public sealed record MonthlyRevenueDto(string Month, decimal Revenue);
public sealed record CategoryDistributionDto(string Category, int Count);
public sealed record RecentBookingDto(
    Guid Id, 
    string BookingReference, 
    string? UserName, 
    string? ServiceName, 
    DateTime Date, 
    decimal Amount, 
    string Currency,
    string Status);
