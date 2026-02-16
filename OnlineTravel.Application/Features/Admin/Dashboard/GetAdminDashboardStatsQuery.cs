using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Admin.Dashboard;

public sealed record GetAdminDashboardStatsQuery : IRequest<Result<AdminDashboardResponse>>;
