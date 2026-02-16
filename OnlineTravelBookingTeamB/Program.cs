using Microsoft.AspNetCore.Identity;
using Ecommerce_Project.Extensions;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Mapping;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;
using Microsoft.Extensions.DependencyInjection; // Ensure this is present



var builder = WebApplication.CreateBuilder(args);

// Configure Serilog Logging
builder.ConfigureSerilog();

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

// Add Health Checks
builder.Services.AddAppHealthChecks();

// Add File Service

var app = builder.Build();
app.UseStaticFiles();

// Enable Serilog Request Logging 
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Generate Swagger JSON
    app.UseSwaggerUI(c =>         // Swagger UI endpoint
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Travel API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseMiddleware<ExceptionMiddleware>();

await app.ApplyDatabaseSetupAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1"));
}

app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
