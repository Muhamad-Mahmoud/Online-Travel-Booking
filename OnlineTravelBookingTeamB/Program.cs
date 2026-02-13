using Ecommerce_Project.Extensions;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Mapping;
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
MapsterConfig.Register();
builder.Services.AddSwaggerGenJwtAuth();
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// A
await app.ApplyDatabaseMigrationsAsync();


// Configure the HTTP request pipeline.
app.UseMiddleware<OnlineTravelBookingTeamB.Middlewares.ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();

app.Run();
