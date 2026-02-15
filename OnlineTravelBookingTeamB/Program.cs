using Serilog;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Identity;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravel.Infrastructure.Services;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog Logging
builder.ConfigureSerilog();

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add Health Checks
builder.Services.AddAppHealthChecks();

// Add File Service

var app = builder.Build();

// Enable Serilog Request Logging 
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

await app.ApplyDatabaseSetupAsync();

app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
