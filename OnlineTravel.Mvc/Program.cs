using OnlineTravel.Application.DependencyInjection;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Infrastructure;
using OnlineTravel.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add Infrastructure Services 
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services 
builder.Services.AddApplication();

// Add Identity configuration if needed (assuming defaults for now, or mirroring Api if appropriate)
// builder.Services.AddIdentity... (Infrastructure might already do some of this)

// Add File Service
var wwwRoot = builder.Environment.WebRootPath
	?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (!Directory.Exists(Path.Combine(wwwRoot, "uploads")))
{
	Directory.CreateDirectory(Path.Combine(wwwRoot, "uploads"));
}
builder.Services.AddScoped<IFileService>(_ => new FileService(wwwRoot));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Admin}/{action=Index}/{id?}");

app.Run();
