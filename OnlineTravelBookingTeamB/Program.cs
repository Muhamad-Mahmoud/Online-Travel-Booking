using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravelBookingTeamB.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();

app.Run();
