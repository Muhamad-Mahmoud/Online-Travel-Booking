using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Api.Extensions;
using OnlineTravel.Api.Middleware;
using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Services;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog Logging
builder.ConfigureSerilog();

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);
//Add Google login
builder.Services
	.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
	.AddCookie(IdentityConstants.ExternalScheme)
	.AddGoogle("Google", options =>
	{
		options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
		options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
		options.CallbackPath = "/signin-google";
		options.SignInScheme = IdentityConstants.ExternalScheme;
		options.Scope.Add("profile");
		options.Scope.Add("email");

	});

// Add Application Services 
builder.Services.AddApplication();

// Add API Services
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddOpenApi();

// Add Health Checks
builder.Services.AddAppHealthChecks();

// Add File Service
var wwwRoot = builder.Environment.WebRootPath
	?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(Path.Combine(wwwRoot, "uploads"));
builder.Services.AddScoped<IFileService>(_ => new FileService(wwwRoot));


builder.Services.AddSwaggerGenJwtAuth();
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

app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
