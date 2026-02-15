using Microsoft.OpenApi.Models;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;

using OnlineTravelBookingTeamB.Middleware;
using OnlineTravel.Infrastructure.Identity;
using OnlineTravel.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add File Service
var webRootPath = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
builder.Services.AddScoped<IFileService>(_ => new FileService(webRootPath));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Online Travel API",
        Version = "v1",
        Description = "API for Multi-category Travel Platform (Hotels, Rooms, Tours, Flights)"
    });
});
var app = builder.Build();
app.UseStaticFiles();

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

await app.ApplyDatabaseMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Data Seeding
    await app.SeedDatabaseAsync();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

await IdentityBootstrapper.InitializeAsync(app.Services);


app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();

app.Run();
