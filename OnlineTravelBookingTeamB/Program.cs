using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravelBookingTeamB.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure Services (Database)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
