using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
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
