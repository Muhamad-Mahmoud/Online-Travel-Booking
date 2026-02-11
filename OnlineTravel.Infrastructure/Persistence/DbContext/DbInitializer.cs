using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace OnlineTravel.Infrastructure.Persistence.DbContext;

public static class DbInitializer
{
    public static async Task SeedAsync(OnlineTravelDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        await context.Database.MigrateAsync();

        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
        await SeedCategoriesAsync(context);
        await SeedToursAsync(context);
        await SeedHotelsAsync(context);
        await SeedFlightsAsync(context);
        await SeedCarsAsync(context);
        await SeedReviewsAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Customer"));
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        if (!await userManager.Users.AnyAsync(u => u.UserName == "admin@onlinetravel.com"))
        {
            var admin = new User
            {
                UserName = "admin@onlinetravel.com",
                Email = "admin@onlinetravel.com",
                Name = "Admin User",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Password123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!await userManager.Users.AnyAsync(u => u.UserName == "customer@onlinetravel.com"))
        {
            var customer = new User
            {
                UserName = "customer@onlinetravel.com",
                Email = "customer@onlinetravel.com",
                Name = "John Doe",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(customer, "Password123!");
            await userManager.AddToRoleAsync(customer, "Customer");
        }
    }

    private static async Task SeedCategoriesAsync(OnlineTravelDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new Category { Type = CategoryType.Tour, Title = "Tours", Description = "Exciting tours and activities", IsActive = true, Image = new ImageUrl("https://images.unsplash.com/photo-1503177119275-0aa32b3a9368", "Tours") },
            new Category { Type = CategoryType.Hotel, Title = "Hotels", Description = "Comfortable stays and resorts", IsActive = true, Image = new ImageUrl("https://images.unsplash.com/photo-1566073771259-6a8506099945", "Hotels") },
            new Category { Type = CategoryType.Flight, Title = "Flights", Description = "Air travel to any destination", IsActive = true, Image = new ImageUrl("https://images.unsplash.com/photo-1436491865332-7a61a109cc05", "Flights") },
            new Category { Type = CategoryType.Car, Title = "Cars", Description = "Vehicle rentals for your journey", IsActive = true, Image = new ImageUrl("https://images.unsplash.com/photo-1549317661-bd32c8ce0db2", "Cars") }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedToursAsync(OnlineTravelDbContext context)
    {
        if (await context.Tours.AnyAsync()) return;

        var tourCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Tour);

        var tours = new List<Tour>
        {
            new Tour
            {
                Title = "Nile River Adventure",
                Description = "A thrilling boat trip down the historic Nile river.",
                CategoryId = tourCategory.Id,
                Recommended = true,
                Address = new Address("Cairo", "Cairo", "Cairo", "Egypt", "12345"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1503177119275-0aa32b3a9368", "Nile River"),
                Highlights = new List<string> { "Sunset cruise", "Ancient ruins", "Local dinner" },
                Tags = new List<string> { "Egypt", "Nile", "River" }
            },
            new Tour
            {
                Title = "Pyramids Quad Bike Safari",
                Description = "Experience the Giza pyramids like never before on a quad bike.",
                CategoryId = tourCategory.Id,
                Recommended = true,
                Address = new Address("Giza", "Giza", "Giza", "Egypt", "54321"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1539768942893-daf53e448371", "Great Pyramids"),
                Highlights = new List<string> { "Quad biking", "Desert views", "Great Sphinx visit" },
                Tags = new List<string> { "Desert", "Safari", "Adventure" }
            },
            new Tour
            {
                Title = "Luxor Hot Air Balloon",
                Description = "Soar over the Valley of the Kings at sunrise.",
                CategoryId = tourCategory.Id,
                Recommended = true,
                Address = new Address("Luxor West Bank", "Luxor", "Luxor", "Egypt", "85111"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1518709268805-4e9042af9f23", "Hot Air Balloon"),
                Highlights = new List<string> { "Sunrise view", "Aerial photography", "Certificate of flight" },
                Tags = new List<string> { "Luxor", "Sunrise", "Balloon" }
            },
            new Tour
            {
                Title = "Karnak Temple Evening Tour",
                Description = "Explore the largest religious complex in the world under the stars.",
                CategoryId = tourCategory.Id,
                Recommended = false,
                Address = new Address("Karnak", "Luxor", "Luxor", "Egypt", "85111"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1572252017416-22a21132f831", "Karnak Temple"),
                Highlights = new List<string> { "Sound and light show", "Expert guide", "Hieroglyphs insights" },
                Tags = new List<string> { "History", "Luxor", "Temples" }
            }
        };

        await context.Tours.AddRangeAsync(tours);
        await context.SaveChangesAsync();

        foreach (var tour in tours)
        {
            var priceTier = new TourPriceTier
            {
                TourId = tour.Id,
                Name = "Standard Package",
                Description = "Standard comprehensive package",
                Price = new Money(100, "USD"),
                IsActive = true
            };
            await context.TourPriceTiers.AddAsync(priceTier);
            await context.SaveChangesAsync();

            var schedule = new TourSchedule
            {
                TourId = tour.Id,
                PriceTierId = priceTier.Id,
                DateRange = new DateRange(DateOnly.FromDateTime(DateTime.Now.AddDays(7)), DateOnly.FromDateTime(DateTime.Now.AddDays(14))),
                TotalCapacity = 20,
                AvailableSlots = 20,
                Status = TourScheduleStatus.Active
            };
            await context.TourSchedules.AddAsync(schedule);
        }
        await context.SaveChangesAsync();
    }

    private static async Task SeedHotelsAsync(OnlineTravelDbContext context)
    {
        if (await context.Hotels.AnyAsync()) return;

        var hotelCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Hotel);

        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Name = "Grand Plaza Hotel",
                Description = "Luxury hotel in the heart of the city.",
                CategoryId = hotelCategory.Id,
                Address = new Address("Downtown", "Cairo", "Cairo", "Egypt", "11111"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1566073771259-6a8506099945", "Hotel Exterior"),
                StarRating = new StarRating(5),
                Amenities = new List<string> { "WiFi", "Pool", "Spa", "Gym" }
            },
            new Hotel
            {
                Name = "Luxor Palace Resort",
                Description = "Escape to luxury on the banks of the Nile.",
                CategoryId = hotelCategory.Id,
                Address = new Address("Nile Corniche", "Luxor", "Luxor", "Egypt", "85111"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1542314831-068cd1dbfeeb", "Luxor Resort"),
                StarRating = new StarRating(5),
                Amenities = new List<string> { "Nile View", "Infinity Pool", "Fine Dining" }
            },
            new Hotel
            {
                Name = "Old Cataract Aswan",
                Description = "A historic gem overlooking the iconic cataracts.",
                CategoryId = hotelCategory.Id,
                Address = new Address("Abtal El Tahrir", "Aswan", "Aswan", "Egypt", "81511"),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1520250497591-112f2f40a3f4", "Aswan View"),
                StarRating = new StarRating(5),
                Amenities = new List<string> { "Historic Library", "Sunset Terrace", "Butler Service" }
            }
        };

        await context.Hotels.AddRangeAsync(hotels);
        await context.SaveChangesAsync();

        foreach (var hotel in hotels)
        {
            var rooms = new List<Room>
            {
                new Room
                {
                    HotelId = hotel.Id,
                    RoomNumber = "101",
                    RoomType = "Luxury Suite",
                    BasePrice = new Money(250, "USD"),
                    MaxGuests = 2,
                    Status = RoomStatus.Active
                },
                new Room
                {
                    HotelId = hotel.Id,
                    RoomNumber = "202",
                    RoomType = "Deluxe Double",
                    BasePrice = new Money(150, "USD"),
                    MaxGuests = 3,
                    Status = RoomStatus.Active
                },
                 new Room
                {
                    HotelId = hotel.Id,
                    RoomNumber = "303",
                    RoomType = "Standard King",
                    BasePrice = new Money(120, "USD"),
                    MaxGuests = 2,
                    Status = RoomStatus.Active
                }
            };
            await context.Rooms.AddRangeAsync(rooms);
        }
        await context.SaveChangesAsync();
    }

    private static async Task SeedFlightsAsync(OnlineTravelDbContext context)
    {
        if (await context.Flights.AnyAsync()) return;

        var flightCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Flight);

        var carriers = new List<Carrier>
        {
            new Carrier { Name = "EgyptAir", Code = new IataCode("MS"), IsActive = true },
            new Carrier { Name = "Emirates", Code = new IataCode("EK"), IsActive = true },
            new Carrier { Name = "Air France", Code = new IataCode("AF"), IsActive = true }
        };
        await context.Carriers.AddRangeAsync(carriers);

        var airports = new List<Airport>
        {
            new Airport
            {
                Name = "Cairo International Airport",
                Code = new IataCode("CAI"),
                Address = new Address("Airport Road", "Cairo", "Cairo", "Egypt", "12345"),
                IsActive = true
            },
            new Airport
            {
                Name = "London Heathrow Airport",
                Code = new IataCode("LHR"),
                Address = new Address("Longford", "London", "London", "UK", "TW6"),
                IsActive = true
            },
            new Airport
            {
                Name = "Dubai International Airport",
                Code = new IataCode("DXB"),
                Address = new Address("Al Garhoud", "Dubai", "Dubai", "UAE", "00000"),
                IsActive = true
            },
            new Airport
            {
                Name = "Paris Charles de Gaulle Airport",
                Code = new IataCode("CDG"),
                Address = new Address("Roissy-en-France", "Paris", "Paris", "France", "95700"),
                IsActive = true
            }
        };
        await context.Airports.AddRangeAsync(airports);
        await context.SaveChangesAsync();

        var msCarrier = carriers.First(c => c.Code == "MS");
        var ekCarrier = carriers.First(c => c.Code == "EK");
        var caiAirport = airports.First(a => a.Code == "CAI");
        var lhrAirport = airports.First(a => a.Code == "LHR");
        var dxbAirport = airports.First(a => a.Code == "DXB");

        var flights = new List<Flight>
        {
             new Flight
            {
                FlightNumber = new FlightNumber("MS777"),
                CarrierId = msCarrier.Id,
                OriginAirportId = caiAirport.Id,
                DestinationAirportId = lhrAirport.Id,
                CategoryId = flightCategory.Id,
                Schedule = new OnlineTravel.Domain.Entities._Shared.ValueObjects.DateTimeRange(DateTime.Now.AddDays(7), DateTime.Now.AddDays(7).AddHours(5)),
                Status = FlightStatus.Scheduled,
                BaggageRules = new List<string> { "23kg check-in", "7kg carry-on" }
            },
            new Flight
            {
                FlightNumber = new FlightNumber("EK902"),
                CarrierId = ekCarrier.Id,
                OriginAirportId = caiAirport.Id,
                DestinationAirportId = dxbAirport.Id,
                CategoryId = flightCategory.Id,
                Schedule = new OnlineTravel.Domain.Entities._Shared.ValueObjects.DateTimeRange(DateTime.Now.AddDays(10), DateTime.Now.AddDays(10).AddHours(3)),
                Status = FlightStatus.Scheduled,
                BaggageRules = new List<string> { "40kg check-in", "10kg carry-on" }
            }
        };
        await context.Flights.AddRangeAsync(flights);
        await context.SaveChangesAsync();

        foreach (var flight in flights)
        {
            var fare = new FlightFare
            {
                FlightId = flight.Id,
                BasePrice = flight.FlightNumber == "EK902" ? new Money(1200, "USD") : new Money(600, "USD"),
                SeatsAvailable = 100
            };
            await context.FlightFares.AddAsync(fare);

            var seats = new List<FlightSeat>
            {
                new FlightSeat { FlightId = flight.Id, SeatLabel = "1A", CabinClass = CabinClass.Business, IsAvailable = true, ExtraCharge = new Money(50, "USD") },
                new FlightSeat { FlightId = flight.Id, SeatLabel = "10B", CabinClass = CabinClass.Economy, IsAvailable = true },
                new FlightSeat { FlightId = flight.Id, SeatLabel = "2C", CabinClass = CabinClass.Business, IsAvailable = true, ExtraCharge = new Money(50, "USD") }
            };
            await context.FlightSeats.AddRangeAsync(seats);
        }
        await context.SaveChangesAsync();
    }

    private static async Task SeedCarsAsync(OnlineTravelDbContext context)
    {
        if (await context.Cars.AnyAsync()) return;

        var carCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Car);

        var brands = new List<CarBrand>
        {
            new CarBrand { Name = "Toyota", IsActive = true },
            new CarBrand { Name = "BMW", IsActive = true },
            new CarBrand { Name = "Mercedes", IsActive = true }
        };
        await context.CarBrands.AddRangeAsync(brands);
        await context.SaveChangesAsync();

        var toyota = brands.First(b => b.Name == "Toyota");
        var bmw = brands.First(b => b.Name == "BMW");
        var mercedes = brands.First(b => b.Name == "Mercedes");

        var cars = new List<Car>
        {
            new Car
            {
                BrandId = toyota.Id,
                Make = "Toyota",
                Model = "Camry",
                CarType = CarCategory.Sedan,
                SeatsCount = 5,
                FuelType = FuelType.Hybrid,
                Transmission = TransmissionType.Automatic,
                CategoryId = carCategory.Id,
                Features = new List<string> { "Bluetooth", "Backup Camera", "Sunroof" },
                Location = new Point(0, 0) { SRID = 4326 }
            },
            new Car
            {
                BrandId = bmw.Id,
                Make = "BMW",
                Model = "X5",
                CarType = CarCategory.SUV,
                SeatsCount = 5,
                FuelType = FuelType.Gasoline,
                Transmission = TransmissionType.Automatic,
                CategoryId = carCategory.Id,
                Features = new List<string> { "Leather Seats", "AWD", "Navigation" },
                Location = new Point(0, 0) { SRID = 4326 }
            },
            new Car
            {
                BrandId = mercedes.Id,
                Make = "Mercedes",
                Model = "C-Class",
                CarType = CarCategory.Luxury,
                SeatsCount = 5,
                FuelType = FuelType.Gasoline,
                Transmission = TransmissionType.Automatic,
                CategoryId = carCategory.Id, // Reusing luxury from categories
                Features = new List<string> { "Ambience Lighting", "Burmester Audio", "Autonomous Driving" },
                Location = new Point(0, 0) { SRID = 4326 }
            }
        };
        await context.Cars.AddRangeAsync(cars);
        await context.SaveChangesAsync();

        foreach (var car in cars)
        {
            var pricingTier = new CarPricingTier
            {
                CarId = car.Id,
                FromHours = 1,
                ToHours = 24,
                PricePerHour = car.Model == "Camry" ? new Money(10, "USD") : new Money(25, "USD")
            };
            await context.CarPricingTiers.AddAsync(pricingTier);

            var extra = new CarExtra
            {
                CarId = car.Id,
                Name = "Full Insurance",
                PricePerDay = new Money(15, "USD"),
                Available = true
            };
            await context.CarExtras.AddAsync(extra);
        }
        await context.SaveChangesAsync();
    }

    private static async Task SeedReviewsAsync(OnlineTravelDbContext context)
    {
        if (await context.Reviews.AnyAsync()) return;

        var user = await context.Users.FirstAsync(u => u.UserName == "customer@onlinetravel.com");
        var tours = await context.Tours.Take(2).ToListAsync();
        var hotels = await context.Hotels.Take(2).ToListAsync();

        var tourCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Tour);
        var hotelCategory = await context.Categories.FirstAsync(c => c.Type == CategoryType.Hotel);

        var reviews = new List<Review>();

        foreach (var tour in tours)
        {
            reviews.Add(new Review
            {
                UserId = user.Id,
                ItemId = tour.Id,
                CategoryId = tourCategory.Id,
                Rating = new StarRating(5),
                Comment = $"Excellent tour of {tour.Title}! Highly recommended."
            });
        }

        foreach (var hotel in hotels)
        {
            reviews.Add(new Review
            {
                UserId = user.Id,
                ItemId = hotel.Id,
                CategoryId = hotelCategory.Id,
                Rating = new StarRating(4.5m),
                Comment = $"I had a great stay at {hotel.Name}. Staff were very helpful."
            });
        }

        await context.Reviews.AddRangeAsync(reviews);
        await context.SaveChangesAsync();
    }
}
