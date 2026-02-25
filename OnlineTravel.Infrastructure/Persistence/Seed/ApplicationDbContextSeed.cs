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
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Reviews;

namespace OnlineTravel.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(OnlineTravelDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager, context);

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
        await SeedBookingsAndReviewsAsync(context, categories);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in new[] { "Admin", "User", "Customer" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager, OnlineTravelDbContext context)
    {
        if (await userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var admin = new AppUser { UserName = "admin@admin.com", Email = "admin@admin.com", Name = "Admin Authority", EmailConfirmed = true, Status = UserStatus.Active };
            if ((await userManager.CreateAsync(admin, "Admin@123")).Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (await userManager.FindByEmailAsync("customer@onlinetravel.com") == null)
        {
            var customer = new AppUser { UserName = "customer@onlinetravel.com", Email = "customer@onlinetravel.com", Name = "Ahmad Mahmoud", EmailConfirmed = true, Status = UserStatus.Active };
            if ((await userManager.CreateAsync(customer, "Password123!")).Succeeded) await userManager.AddToRoleAsync(customer, "Customer");
        }
    }

    private static async Task SeedToursAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Tours.CountAsync() >= 8) return;

        var existingTitles = await context.Tours.Select(t => t.Title).ToListAsync();
        var tours = new List<Tour>();

        void AddTour(string title, string desc, int days, int nights, string city, string img, List<string> highlights)
        {
            if (!existingTitles.Any(ct => ct.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                tours.Add(new Tour
                {
                    Title = title,
                    Description = desc,
                    CategoryId = categoryId,
                    DurationDays = days,
                    DurationNights = nights,
                    Address = new Address("Downtown", city, city, "Egypt", "12345", new Point(31.1, 30.0) { SRID = 4326 }),
                    MainImage = new ImageUrl(img, title),
                    Highlights = highlights,
                    Tags = new List<string> { "Premium", "Cultural", "Recommended" },
                    Recommended = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        AddTour("Giza Pyramids Authority", "Official guided tour of the Great Pyramids and Sphinx.", 1, 0, "Giza", "https://images.unsplash.com/photo-1503177119275-0aa32b3a9368", new List<string>{"Sunset Camel Ride", "Great Pyramid Entry", "Expert Archeologist"});
        AddTour("Nile Majesty Cruise", "Luxury 5-star cruise from Luxor to Aswan with all excursions.", 4, 3, "Luxor", "https://images.unsplash.com/photo-1544955214-e0e64c12659e", new List<string>{"Valley of the Kings", "Karnak Temple", "Galabeya Party"});
        AddTour("Red Sea Divers Secret", "Hidden diving spots in Hurghada with professional instructors.", 3, 2, "Hurghada", "https://images.unsplash.com/photo-1544551763-46a013bb70d5", new List<string>{"PADI Certified", "Night Dive", "Coral Reef Photo"});
        AddTour("Siwa Oasis Nomad Life", "Authentic Berber experience in the heart of the Sahara.", 5, 4, "Siwa", "https://images.unsplash.com/photo-1509316785289-025f5d846b35", new List<string>{"Salt Lakes", "Cleopatra's Bath", "Desert Glamping"});
        AddTour("Abbott Monastery Peak", "Spiritual hike to the summit of Mt. Sinai.", 1, 1, "Saint Catherine", "https://images.unsplash.com/photo-1548543604-a87c9909abec", new List<string>{"Sunrise Prayer", "St. Catherine Monastery", "Hermit Path"});
        AddTour("Alexandrian Heritage", "The library, the citadel, and the sunken city.", 2, 1, "Alexandria", "https://images.unsplash.com/photo-1554593453-29f95d52252a", new List<string>{"Library Tour", "Citadel View", "Seafood Dinner"});

        if (tours.Any())
        {
            context.Tours.AddRange(tours);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedHotelsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Hotels.CountAsync() >= 8) return;

        var existingSlugs = await context.Hotels.Select(h => h.Slug).ToListAsync();
        var hotels = new List<Hotel>();

        void AddHotel(string name, string slug, string desc, string city, string img)
        {
            if (!existingSlugs.Any(s => s.Equals(slug, StringComparison.OrdinalIgnoreCase)))
            {
                var hotel = new Hotel(
                    name, slug, desc,
                    new Address("Plaza Avenue", city, city, "Egypt", "11211", new Point(31.2, 30.0) { SRID = 4326 }),
                    new ContactInfo(new EmailAddress($"concierge@{slug}.com"), new PhoneNumber("+2010000000")),
                    new TimeRange(new TimeOnly(14, 0), new TimeOnly(23, 59)),
                    new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0)),
                    "Non-refundable after check-in",
                    img
                );
                
                // Add diverse rooms
                hotel.AddRoom(new Room(hotel.Id, "P1", "Presidential Suite", "Top floor with private spa", new Money(1200, "USD")));
                hotel.AddRoom(new Room(hotel.Id, "D1", "Deluxe Twin", "City view, high floor", new Money(180, "USD")));
                hotel.AddRoom(new Room(hotel.Id, "S1", "Standard King", "Garden view", new Money(130, "USD")));
                
                hotels.Add(hotel);
            }
        }

        AddHotel("Cairo Marriott Grand", "cairo-marriott-grand", "Historical palace turned luxury hotel.", "Cairo", "https://images.unsplash.com/photo-1566073771259-6a8506099945");
        AddHotel("Four Seasons Resort Sharm", "fs-sharm-resort", "Red Sea paradise for elite travelers.", "Sharm El Sheikh", "https://images.unsplash.com/photo-1571003123894-1f0594d2b5d9");
        AddHotel("Steigenberger Nile Palace", "nile-palace-luxor", "Classic elegance overlooking the Nile.", "Luxor", "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb");
        AddHotel("Hilton Mediterranean View", "hilton-alex-view", "Modern stay at the Alexandria corniche.", "Alexandria", "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4");
        AddHotel("Movenpick Island Resort", "movenpick-aswan-resort", "Private Elephantine island experience.", "Aswan", "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b");

        if (hotels.Any())
        {
            context.Hotels.AddRange(hotels);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCarsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Cars.CountAsync() >= 8) return;

        var brands = new Dictionary<string, CarBrand>();
        foreach (var name in new[] { "Hyundai", "Toyota", "BMW", "Mercedes", "Kia", "Tesla" })
        {
            var brand = await context.CarBrands.FirstOrDefaultAsync(b => b.Name == name);
            if (brand == null)
            {
                brand = new CarBrand { Name = name, Logo = $"{name.ToLower()}.png", CreatedAt = DateTime.UtcNow };
                context.CarBrands.Add(brand);
                await context.SaveChangesAsync();
            }
            brands[name] = brand;
        }

        var existingModels = await context.Cars.Select(c => c.Model).ToListAsync();
        var cars = new List<Car>();

        void AddCar(string brandName, string model, CarCategory cat, int seats, FuelType fuel, Money price)
        {
            if (!existingModels.Any(m => m.Equals(model, StringComparison.OrdinalIgnoreCase)))
            {
                cars.Add(new Car
                {
                    BrandId = brands[brandName].Id,
                    Make = brandName,
                    Model = model,
                    CarType = cat,
                    SeatsCount = seats,
                    FuelType = fuel,
                    Transmission = TransmissionType.Automatic,
                    CategoryId = categoryId,
                    Location = new Point(31.2, 30.0) { SRID = 4326 },
                    PricingTiers = new List<CarPricingTier> { new CarPricingTier { PricePerHour = price, FromHours = 1, ToHours = 168 } }
                });
            }
        }

        AddCar("Tesla", "Model S", CarCategory.Luxury, 5, FuelType.Electric, new Money(2500, "EGP"));
        AddCar("BMW", "X7", CarCategory.SUV, 7, FuelType.Petrol, new Money(3500, "EGP"));
        AddCar("Hyundai", "Sonata", CarCategory.Sedan, 5, FuelType.Petrol, new Money(800, "EGP"));
        AddCar("Kia", "Carnival", CarCategory.Van, 8, FuelType.Petrol, new Money(1200, "EGP"));
        AddCar("Mercedes", "EQS", CarCategory.Luxury, 5, FuelType.Electric, new Money(5000, "EGP"));

        if (cars.Any())
        {
            context.Cars.AddRange(cars);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedFlightsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Flights.CountAsync() >= 8) return;

        var rules = new List<FareRule>();
        if (!await context.FareRules.AnyAsync())
        {
            rules.Add(new FareRule { Name = "First Class Elite", Description = "Luxury services, 3x32kg baggage, free seat selection.", CancellationRules = new List<string>{"Full refund anytime", "Free change"} });
            rules.Add(new FareRule { Name = "Business Flex", Description = "Priority boarding, 2x32kg baggage.", CancellationRules = new List<string>{"Refundable 24h before"} });
            rules.Add(new FareRule { Name = "Economy Saver", Description = "Basic seat, 1x23kg baggage.", CancellationRules = new List<string>{"No refund", "Change fee applies"} });
            context.FareRules.AddRange(rules);
            await context.SaveChangesAsync();
        }
        else rules = await context.FareRules.ToListAsync();

        var carriers = new Dictionary<string, Carrier>();
        foreach (var (n, c) in new[] { ("EgyptAir", "MS"), ("Emirates", "EK"), ("Turkish", "TK"), ("Lufthansa", "LH"), ("British Airways", "BA") })
        {
            var carrier = await context.Carriers.FirstOrDefaultAsync(car => car.Code.Value == c);
            if (carrier == null)
            {
                carrier = new Carrier { Name = n, Code = new IataCode(c), IsActive = true };
                context.Carriers.Add(carrier);
                await context.SaveChangesAsync();
            }
            carriers[c] = carrier;
        }

        var airports = new Dictionary<string, Airport>();
        foreach (var (n, c, l) in new[] { ("Cairo Int", "CAI", "Cairo"), ("Heathrow", "LHR", "London"), ("JFK Int", "JFK", "New York"), ("Dubai Int", "DXB", "Dubai"), ("Istanbul", "IST", "Istanbul") })
        {
            var airport = await context.Airports.FirstOrDefaultAsync(a => a.Code.Value == c);
            if (airport == null)
            {
                airport = new Airport { Name = n, Code = new IataCode(c), Address = new Address("Airport Rd", l, l, "Country", "11776", new Point(31.4, 30.1) { SRID = 4326 }) };
                context.Airports.Add(airport);
                await context.SaveChangesAsync();
            }
            airports[c] = airport;
        }

        var existingNums = await context.Flights.Select(f => f.FlightNumber.Value).ToListAsync();
        var flights = new List<Flight>();

        void AddFlight(string num, string carCode, string orig, string dest, Money basePrice)
        {
            if (!existingNums.Any(n => n.Equals(num, StringComparison.OrdinalIgnoreCase)))
            {
                var fl = new Flight
                {
                    FlightNumber = new FlightNumber(num),
                    CarrierId = carriers[carCode].Id,
                    OriginAirportId = airports[orig].Id,
                    DestinationAirportId = airports[dest].Id,
                    Schedule = new DateTimeRange(DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(15).AddHours(6)),
                    CategoryId = categoryId,
                    Status = FlightStatus.Scheduled,
                    BaggageRules = new List<string>{"Checked: 2x23kg", "Cabin: 1x7kg"},
                    Refundable = true
                };

                fl.Fares.Add(new FlightFare { BasePrice = basePrice, SeatsAvailable = 200, FareRulesId = rules.OrderBy(r => Guid.NewGuid()).FirstOrDefault()?.Id });
                
                for(int r = 1; r <= 3; r++) {
                    fl.Seats.Add(new FlightSeat { SeatLabel = $"{r}A", CabinClass = CabinClass.First, ExtraCharge = new Money(500, "USD") });
                    fl.Seats.Add(new FlightSeat { SeatLabel = $"{r+10}B", CabinClass = CabinClass.Economy });
                }

                flights.Add(fl);
            }
        }

        AddFlight("MS985", "MS", "CAI", "JFK", new Money(1200, "USD"));
        AddFlight("EK202", "EK", "CAI", "DXB", new Money(300, "USD"));
        AddFlight("BA155", "BA", "LHR", "CAI", new Money(550, "USD"));
        AddFlight("TK001", "TK", "IST", "LHR", new Money(400, "USD"));

        if (flights.Any()) { context.Flights.AddRange(flights); await context.SaveChangesAsync(); }
    }

    private static async Task SeedBookingsAndReviewsAsync(OnlineTravelDbContext context, List<Category> categories)
    {
        if (await context.Bookings.AnyAsync()) return;

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "customer@onlinetravel.com");
        if (user == null) return;

        var toursCat = categories.FirstOrDefault(c => c.Type == CategoryType.Tour);
        var hotelsCat = categories.FirstOrDefault(c => c.Type == CategoryType.Hotel);
        
        var sampleTour = await context.Tours.FirstOrDefaultAsync();
        var sampleHotel = await context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync();

        if (sampleTour != null)
        {
            var b1 = BookingEntity.Create(user.Id, new Money(500, "USD"));
            b1.ConfirmPayment("pi_tour_seed");
            b1.AddDetail(BookingDetail.Create(toursCat!.Id, sampleTour.Id, sampleTour.Title, new DateTimeRange(DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(7))));
            context.Bookings.Add(b1);

            context.Reviews.Add(new Review { CategoryId = toursCat.Id, ItemId = sampleTour.Id, UserId = user.Id, Rating = new StarRating(5), Comment = "Absolutely magnificent tour! The guide was world-class.", CreatedAt = DateTime.UtcNow });
        }

        if (sampleHotel != null)
        {
            var b2 = BookingEntity.Create(user.Id, new Money(300, "USD"));
            b2.AddDetail(BookingDetail.Create(hotelsCat!.Id, sampleHotel.Id, sampleHotel.Name, new DateTimeRange(DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(15))));
            context.Bookings.Add(b2);

            context.Reviews.Add(new Review { CategoryId = hotelsCat.Id, ItemId = sampleHotel.Id, HotelId = sampleHotel.Id, UserId = user.Id, Rating = new StarRating(4), Comment = "Wonderful service, but the pool was a bit cold.", CreatedAt = DateTime.UtcNow });
        }

        await context.SaveChangesAsync();
    }
}
