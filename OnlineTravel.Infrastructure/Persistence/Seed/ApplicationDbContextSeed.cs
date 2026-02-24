using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Persistence.Context;
using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;

namespace OnlineTravel.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(OnlineTravelDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);

        if (!await context.Categories.AnyAsync())
        {
            context.Categories.AddRange(
                new Category { Title = "Tours", Type = CategoryType.Tour, Description = "Guided tours globally", IsActive = true },
                new Category { Title = "Flights", Type = CategoryType.Flight, Description = "Best flight deals", IsActive = true },
                new Category { Title = "Hotels", Type = CategoryType.Hotel, Description = "Luxury & budget stays", IsActive = true },
                new Category { Title = "Cars", Type = CategoryType.Car, Description = "Reliable car rentals", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        var categories = await context.Categories.ToListAsync();
        var toursCatId = categories.FirstOrDefault(c => c.Title == "Tours")?.Id ?? Guid.Empty;
        var flightsCatId = categories.FirstOrDefault(c => c.Title == "Flights")?.Id ?? Guid.Empty;
        var hotelsCatId = categories.FirstOrDefault(c => c.Title == "Hotels")?.Id ?? Guid.Empty;
        var carsCatId = categories.FirstOrDefault(c => c.Title == "Cars")?.Id ?? Guid.Empty;

        if (toursCatId != Guid.Empty) await SeedToursAsync(context, toursCatId);
        if (hotelsCatId != Guid.Empty) await SeedHotelsAsync(context, hotelsCatId);
        if (carsCatId != Guid.Empty) await SeedCarsAsync(context, carsCatId);
        if (flightsCatId != Guid.Empty) await SeedFlightsAsync(context, flightsCatId);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (await userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var admin = new AppUser { UserName = "admin@admin.com", Email = "admin@admin.com", Name = "Admin User", EmailConfirmed = true, Status = UserStatus.Active };
            if ((await userManager.CreateAsync(admin, "Admin@123")).Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    private static async Task SeedToursAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Tours.AnyAsync()) return;

        var tours = new List<Tour>
        {
            new Tour
            {
                Title = "Exploring the Pyramids",
                Description = "A full day tour to Giza Pyramids and Sphinx.",
                CategoryId = categoryId,
                DurationDays = 1,
                DurationNights = 0,
                Address = new Address("Al Haram", "Giza", "Giza", "Egypt", "12345", new Point(31.1342, 29.9792) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1503177119275-0aa32b3a9368", "Pyramids"),
                Highlights = new List<string> { "Great Pyramid", "The Sphinx", "Camel Ride" },
                Tags = new List<string> { "History", "Adventure" },
                Recommended = true
            },
            new Tour
            {
                Title = "Cruise on the Nile",
                Description = "Luxury cruise from Luxor to Aswan.",
                CategoryId = categoryId,
                DurationDays = 4,
                DurationNights = 3,
                Address = new Address("Kornish Al Nile", "Luxor", "Luxor", "Egypt", "54321", new Point(32.6396, 25.6872) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1544955214-e0e64c12659e", "Nile"),
                Highlights = new List<string> { "Karnak Temple", "Valley of the Kings", "Edfu Temple" },
                Tags = new List<string> { "Luxury", "Sightseeing" }
            }
        };

        context.Tours.AddRange(tours);
        await context.SaveChangesAsync();
    }

    private static async Task SeedHotelsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Hotels.AnyAsync()) return;

        var hotels = new List<Hotel>();

        // Cairo Marriott
        var marriott = new Hotel(
            "Cairo Marriott Hotel",
            "cairo-marriott",
            "Historic luxury in Zamalek.",
            new Address("16 Saray El Gezira", "Cairo", "Cairo", "Egypt", "11211", new Point(31.2248, 30.0571) { SRID = 4326 }),
            new ContactInfo(new EmailAddress("stay@marriott.com"), new PhoneNumber("+20227283000")),
            new TimeRange(new TimeOnly(15, 0), new TimeOnly(23, 0)),
            new TimeRange(new TimeOnly(12, 0), new TimeOnly(13, 0)),
            "Standard cancellation policy",
            "https://images.unsplash.com/photo-1566073771259-6a8506099945"
        );
        marriott.AddRoom(new Room(marriott.Id, "101", "Executive King", "Spacious room with king bed", new Money(250, "USD")));
        marriott.AddRoom(new Room(marriott.Id, "102", "Garden Double", "Direct garden access", new Money(300, "USD")));
        hotels.Add(marriott);

        // Four Seasons Sharm
        var fourSeasons = new Hotel(
            "Four Seasons Resort Sharm El Sheikh",
            "fs-sharm",
            "World-class diving and luxury resort.",
            new Address("1 Four Seasons Blvd", "Sharm El Sheikh", "South Sinai", "Egypt", "46619", new Point(34.3944, 27.9653) { SRID = 4326 }),
            new ContactInfo(new EmailAddress("info@fourseasons.com"), new PhoneNumber("+20693603555")),
            new TimeRange(new TimeOnly(14, 0), new TimeOnly(22, 0)),
            new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0)),
            "Non-refundable during peak season",
            "https://images.unsplash.com/photo-1571003123894-1f0594d2b5d9"
        );
        fourSeasons.AddRoom(new Room(fourSeasons.Id, "V1", "Royal Villa", "Private pool and beachfront", new Money(2000, "USD")));
        hotels.Add(fourSeasons);

        context.Hotels.AddRange(hotels);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCarsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Cars.AnyAsync()) return;

        var brand = new CarBrand { Name = "Hyundai", Logo = "hyundai.png" };
        context.CarBrands.Add(brand);
        await context.SaveChangesAsync();

        var cars = new List<Car>
        {
            new Car
            {
                BrandId = brand.Id,
                Make = "Hyundai",
                Model = "Elantra",
                CarType = CarCategory.Sedan,
                SeatsCount = 5,
                FuelType = FuelType.Petrol,
                Transmission = TransmissionType.Automatic,
                CategoryId = categoryId,
                Location = new Point(31.2357, 30.0444) { SRID = 4326 },
                PricingTiers = new List<CarPricingTier> { new CarPricingTier { PricePerHour = new Money(500, "EGP"), FromHours = 1, ToHours = 168 } }
            }
        };
        context.Cars.AddRange(cars);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFlightsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Flights.AnyAsync()) return;

        var carrier = new Carrier { Name = "EgyptAir", Code = new IataCode("MS"), IsActive = true };
        context.Carriers.Add(carrier);

        var cai = new Airport { Name = "Cairo Int Airport", Code = new IataCode("CAI"), Address = new Address("Heliopolis", "Cairo", "Cairo", "Egypt", "11776", new Point(31.3997, 30.1219) { SRID = 4326 }) };
        var lhr = new Airport { Name = "London Heathrow", Code = new IataCode("LHR"), Address = new Address("Hounslow", "London", "London", "UK", "TW6", new Point(-0.4543, 51.4700) { SRID = 4326 }) };
        context.Airports.AddRange(cai, lhr);
        await context.SaveChangesAsync();

        var flight = new Flight
        {
            FlightNumber = new FlightNumber("MS777"),
            CarrierId = carrier.Id,
            OriginAirportId = cai.Id,
            DestinationAirportId = lhr.Id,
            Schedule = new DateTimeRange(DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(10).AddHours(5)),
            CategoryId = categoryId,
            Status = FlightStatus.Scheduled
        };
        context.Flights.Add(flight);
        await context.SaveChangesAsync();
    }
}
