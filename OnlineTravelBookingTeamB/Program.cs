using Microsoft.AspNetCore.Identity;
using Ecommerce_Project.Extensions;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Mapping;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Identity;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravel.Infrastructure.Services;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;
using OnlineTravel.Infrastructure.Identity;
using OnlineTravel.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection; // Ensure this is present


var builder = WebApplication.CreateBuilder(args);

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

// Add File Service
var webRootPath = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
builder.Services.AddScoped<IFileService>(_ => new FileService(webRootPath));

MapsterConfig.Register();
builder.Services.AddSwaggerGenJwtAuth();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

await app.ApplyDatabaseMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    // Data Seeding
    await app.SeedDatabaseAsync();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

await IdentityBootstrapper.InitializeAsync(app.Services);


app.UseRouting();

app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
