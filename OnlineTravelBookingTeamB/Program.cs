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

// A
await app.ApplyDatabaseMigrationsAsync();


// Configure the HTTP request pipeline.
app.UseMiddleware<OnlineTravelBookingTeamB.Middlewares.ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();

app.Run();
