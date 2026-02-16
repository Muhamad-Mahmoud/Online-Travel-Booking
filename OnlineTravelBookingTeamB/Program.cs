
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Services;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;
using Serilog; // Ensure this is present



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
// builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Online Travel API", Version = "v1" });
});


// Add Health Checks
builder.Services.AddAppHealthChecks();

// رفع الصور يُخزّن في wwwroot/uploads
var wwwRoot = builder.Environment.WebRootPath
    ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(Path.Combine(wwwRoot, "uploads"));
builder.Services.AddScoped<IFileService>(_ => new FileService(wwwRoot));
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

var app = builder.Build();
app.UseStaticFiles();

// Enable Serilog Request Logging 
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

await app.ApplyDatabaseSetupAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Travel Booking API v1"));
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
