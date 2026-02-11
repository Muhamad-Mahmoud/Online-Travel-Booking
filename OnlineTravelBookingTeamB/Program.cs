using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Infrastructure;
using OnlineTravelBookingTeamB.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure Services (Database)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Data Seeding
    await app.SeedDatabaseAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
