using Serilog;
using Microsoft.AspNetCore.Identity;
using Ecommerce_Project.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection; // Ensure this is present
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Application.Mapping;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Identity;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravel.Infrastructure.Services;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;
using Microsoft.Extensions.DependencyInjection; // Ensure this is present
using System.Text.Json;
using System.Text.Json.Serialization;



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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // تحويل أسماء الخصائص إلى camelCase (مثلاً Latitude -> latitude)
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        // تجاهل حالة الأحرف (احتياطي)
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // السماح باستقبال الـ enums كنصوص (strings)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var webRootPath = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
builder.Services.AddScoped<IFileService>(_ => new FileService(webRootPath));

MapsterConfig.Register();
builder.Services.AddSwaggerGenJwtAuth();
var app = builder.Build();

// Enable Serilog Request Logging 
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
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
