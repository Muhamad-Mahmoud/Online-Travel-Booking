using Microsoft.OpenApi.Models;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravelBookingTeamB.Extensions;
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Generate Swagger JSON
    app.UseSwaggerUI(c =>         // Swagger UI endpoint
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Travel API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
