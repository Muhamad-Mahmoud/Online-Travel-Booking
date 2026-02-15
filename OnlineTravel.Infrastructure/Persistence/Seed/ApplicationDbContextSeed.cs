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
        // 1. Seed Roles
        await SeedRolesAsync(roleManager);

        // 2. Seed Users
        await SeedUsersAsync(userManager);

        // 3. Seed Categories
        if (!await context.Categories.AnyAsync())
        {
            var toursCategory = new Category { Title = "Tours", Type = CategoryType.Tour, Description = "Explore the world with our guided tours", IsActive = true };
            var flightsCategory = new Category { Title = "Flights", Type = CategoryType.Flight, Description = "Book flights to anywhere", IsActive = true };
            var hotelsCategory = new Category { Title = "Hotels", Type = CategoryType.Hotel, Description = "Find the best places to stay", IsActive = true };
            var carsCategory = new Category { Title = "Cars", Type = CategoryType.Car, Description = "Rent a car for your journey", IsActive = true };

            context.Categories.AddRange(toursCategory, flightsCategory, hotelsCategory, carsCategory);
            await context.SaveChangesAsync();
        }

        // Retrieve Categories (whether newly created or existing)
        var categories = await context.Categories.ToListAsync();
        var toursCatId = categories.FirstOrDefault(c => c.Title == "Tours")?.Id ?? Guid.Empty;
        var flightsCatId = categories.FirstOrDefault(c => c.Title == "Flights")?.Id ?? Guid.Empty;
        var hotelsCatId = categories.FirstOrDefault(c => c.Title == "Hotels")?.Id ?? Guid.Empty;
        var carsCatId = categories.FirstOrDefault(c => c.Title == "Cars")?.Id ?? Guid.Empty;

        // 4. Seed Tours
        if (toursCatId != Guid.Empty) await SeedToursAsync(context, toursCatId);
        
        // 5. Seed Hotels
        if (hotelsCatId != Guid.Empty) await SeedHotelsAsync(context, hotelsCatId);

        // 6. Seed Cars
        if (carsCatId != Guid.Empty) await SeedCarsAsync(context, carsCatId);

        // 7. Seed Flights
        if (flightsCatId != Guid.Empty) await SeedFlightsAsync(context, flightsCatId);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roles = { "Admin", "User", "Manager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (await userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Name = "System Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                Status = UserStatus.Active
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        if (await userManager.FindByEmailAsync("user@user.com") == null)
        {
            var normalUser = new AppUser
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                Name = "John Doe",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                Status = UserStatus.Active
            };

            var result = await userManager.CreateAsync(normalUser, "User@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }
    }

    private static async Task SeedToursAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        // 1. Paris Tour
        var parisTour = await context.Tours
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(t => t.Title == "Majestic Paris Tour");

        if (parisTour == null)
        {
            parisTour = new Tour
            {
                Title = "Majestic Paris Tour",
                Description = "Experience the magic of Paris with a guided tour of the Eiffel Tower, Louvre Museum, and a Seine River cruise.",
                CategoryId = categoryId,
                DurationDays = 7,
                DurationNights = 6,
                BestTimeToVisit = "Spring (April-June) and autumn (September-October) are perfect times to visit Paris, with mild weather and fewer tourists.",
                Address = new Address(
                    "Champ de Mars",
                    "Paris",
                    "Île-de-France",
                    "France",
                    "75007",
                    new Point(new Coordinate(2.2945, 48.8584)) { SRID = 4326 }
                ),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1511739001486-6bfe10ce7859?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Eiffel Tower"),
                Highlights = new List<string> { "Eiffel Tower Access", "Louvre Museum Skip-the-line", "Seine River Cruise" },
                Tags = new List<string> { "City", "Romance", "History" },
                Recommended = true,
                Activities = new List<TourActivity>
                {
                    new TourActivity
                    {
                        Title = "Visit the Grand Plaza",
                        Description = "The heart of Odisia, surrounded by historical buildings and lively cafes.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1499856871940-a09627c6dcf6?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Grand Plaza")
                    },
                    new TourActivity
                    {
                        Title = "Explore the Museum",
                        Description = "Features modern art by local and global artists.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1544967082-d9d3f9ec671e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Modern Museum")
                    },
                    new TourActivity
                    {
                        Title = "Wander lush, green paths",
                        Description = "A serene oasis featuring diverse plant species and tranquil walking paths.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1585320806297-9794b3e4eeae?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Garden Path")
                    },
                    new TourActivity
                    {
                        Title = "Discover Sunset Bay",
                        Description = "A peaceful coastal spot, perfect for golden views and quiet strolls.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Sunset Bay")
                    }
                }
            };

            parisTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier
                {
                    Name = "Standard",
                    Description = "Standard entry tickets",
                    Price = new Money(150, "EUR"),
                    Schedules = generateSchedules(DateTime.UtcNow.AddDays(10), 2, parisTour)
                },
                new TourPriceTier
                {
                    Name = "VIP",
                    Description = "Skip the line and private guide",
                    Price = new Money(250, "EUR"),
                    Schedules = generateSchedules(DateTime.UtcNow.AddDays(10), 1, parisTour, 5)
                }
            };
            context.Tours.Add(parisTour);
        }
            // Update existing tour
            parisTour.DurationDays = 7;
            parisTour.DurationNights = 6;
            parisTour.BestTimeToVisit = "Spring (April-June) and autumn (September-October) are perfect times to visit Paris, with mild weather and fewer tourists.";
            
            // Ensure Gallery Images are populated
            if (parisTour.Images == null || !parisTour.Images.Any())
            {
                parisTour.Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Paris Street" },
                    new TourImage { Url = "https://images.unsplash.com/photo-1478393806297-c8172945d8b2?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Versailles" },
                    new TourImage { Url = "https://images.unsplash.com/photo-1503917988258-f87a78e3c99d?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Louvre at Night" }
                };
            }

            if (!parisTour.Activities.Any())
            {
                parisTour.Activities = new List<TourActivity>
                {
                    new TourActivity
                    {
                        Title = "Visit the Grand Plaza",
                        Description = "The heart of Odisia, surrounded by historical buildings and lively cafes.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1499856871940-a09627c6dcf6?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Grand Plaza")
                    },
                    new TourActivity
                    {
                        Title = "Explore the Museum",
                        Description = "Features modern art by local and global artists.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1544967082-d9d3f9ec671e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Modern Museum")
                    },
                    new TourActivity
                    {
                        Title = "Wander lush, green paths",
                        Description = "A serene oasis featuring diverse plant species and tranquil walking paths.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1585320806297-9794b3e4eeae?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Garden Path")
                    },
                    new TourActivity
                    {
                        Title = "Discover Sunset Bay",
                        Description = "A peaceful coastal spot, perfect for golden views and quiet strolls.",
                        Image = new ImageUrl("https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Sunset Bay")
                    }
                };
            }


        // ==========================================
        // 2. Tokyo Tour
        // ==========================================
        // ==========================================
        // 2. Tokyo Tour
        // ==========================================
        var tokyoTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "Futuristic Tokyo & Tradition");
        if (tokyoTour == null)
        {
            tokyoTour = new Tour
            {
                Title = "Futuristic Tokyo & Tradition",
                Description = "Immerse yourself in the bustling metropolis of Tokyo, blending neon-lit streets with historic temples.",
                CategoryId = categoryId,
                DurationDays = 9,
                DurationNights = 8,
                BestTimeToVisit = "March to May and September to November.",
                Address = new Address("Shibuya City", "Tokyo", "Tokyo", "Japan", "150-8010", new Point(new Coordinate(139.6917, 35.6895)) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Tokyo Skyline"),
                Highlights = new List<string> { "Shibuya Crossing", "Senso-ji Temple", "Tsukiji Outer Market" },
                Tags = new List<string> { "City", "Culture", "Food" },
                Recommended = true,
                Images = new List<TourImage>
                {
                     new TourImage { Url = "https://images.unsplash.com/photo-1536098561742-ca998e48cbcc?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Shibuya Crossing" },
                     new TourImage { Url = "https://images.unsplash.com/photo-1528360983277-13d9012356ee?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Japan Temple" }
                },
                Activities = new List<TourActivity>
                {
                    new TourActivity { Title = "Sushi Making Class", Description = "Learn to craft authentic sushi with a master chef.", Image = new ImageUrl("https://images.unsplash.com/photo-1579871494447-9811cf80d66c?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Sushi") },
                    new TourActivity { Title = "TeamLab Planets", Description = "Immersive digital art museum experience.", Image = new ImageUrl("https://images.unsplash.com/photo-1558258385-e111246c4342?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Digital Art") }
                }
            };
            tokyoTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier { Name = "Standard", Description = "Guided urban adventure", Price = new Money(2000, "USD"), Schedules = generateSchedules(DateTime.UtcNow.AddDays(20), 3, tokyoTour) }
            };
            context.Tours.Add(tokyoTour);
        }
        else if (tokyoTour.Images == null || !tokyoTour.Images.Any())
        {
             tokyoTour.Images = new List<TourImage>
             {
                  new TourImage { Url = "https://images.unsplash.com/photo-1536098561742-ca998e48cbcc?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Shibuya Crossing" },
                  new TourImage { Url = "https://images.unsplash.com/photo-1528360983277-13d9012356ee?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Japan Temple" }
             };
        }

        // ==========================================
        // 3. New York Tour
        // ==========================================
        var nyTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "Concrete Jungle Adventure");
        if (nyTour == null)
        {
            nyTour = new Tour
            {
                Title = "Concrete Jungle Adventure",
                Description = "The ultimate New York City experience, from the Statue of Liberty to Broadway.",
                CategoryId = categoryId,
                DurationDays = 6,
                DurationNights = 5,
                BestTimeToVisit = "April to June and September to November.",
                Address = new Address("Times Square", "New York", "NY", "USA", "10036", new Point(new Coordinate(-73.9855, 40.7580)) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1496442226666-8d4d0e62e6e9?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Times Square"),
                Highlights = new List<string> { "Times Square", "Statue of Liberty", "Central Park" },
                Tags = new List<string> { "Urban", "Entertainment", "Shopping" },
                Recommended = false,
                Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1522083165195-3424ed129620?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "NYC Skyline" },
                    new TourImage { Url = "https://images.unsplash.com/photo-1543716627-8320f2b380b0?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Statue of Liberty" }
                },
                Activities = new List<TourActivity>
                {
                    new TourActivity { Title = "Broadway Show", Description = "Experience a world-class musical in the theater district.", Image = new ImageUrl("https://images.unsplash.com/photo-1503525145780-87dc45b37648?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Theater") },
                    new TourActivity { Title = "Central Park Bike Tour", Description = "Cycle through the iconic green lung of the city.", Image = new ImageUrl("https://images.unsplash.com/photo-1576406734685-3b95764d1f2e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Bike Tour") }
                }
            };
            nyTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier { Name = "Explorer", Description = "City pass included", Price = new Money(1800, "USD"), Schedules = generateSchedules(DateTime.UtcNow.AddDays(15), 3, nyTour) }
            };
            context.Tours.Add(nyTour);
        }
        else if (nyTour.Images == null || !nyTour.Images.Any())
        {
            nyTour.Images = new List<TourImage>
            {
                new TourImage { Url = "https://images.unsplash.com/photo-1522083165195-3424ed129620?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "NYC Skyline" },
                new TourImage { Url = "https://images.unsplash.com/photo-1543716627-8320f2b380b0?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Statue of Liberty" }
            };
        }

        // ==========================================
        // 4. Bali Tour
        // ==========================================
        // ==========================================
        // 4. Bali Tour
        // ==========================================
        var baliTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "Bali Island Escape");
        if (baliTour == null)
        {
            baliTour = new Tour
            {
                Title = "Bali Island Escape",
                Description = "Relax on pristine beaches, explore lush rice terraces, and discover spiritual temples.",
                CategoryId = categoryId,
                DurationDays = 8,
                DurationNights = 7,
                BestTimeToVisit = "April to October.",
                Address = new Address("Ubud", "Gianyar", "Bali", "Indonesia", "80571", new Point(new Coordinate(115.2625, -8.5069)) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1537996194471-e657df975ab4?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Bali Temple"),
                Highlights = new List<string> { "Ubud Monkey Forest", "Uluwatu Temple", "Tegallalang Rice Terrace" },
                Tags = new List<string> { "Nature", "Relaxation", "Beach" },
                Recommended = true,
                Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Bali Rice Field" },
                    new TourImage { Url = "https://images.unsplash.com/photo-1518548419970-58e3b4079ab2?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Bali Beach" }
                },
                Activities = new List<TourActivity>
                {
                    new TourActivity { Title = "Sunrise Yoga", Description = "Start your day with zen yoga overlooking the jungle.", Image = new ImageUrl("https://images.unsplash.com/photo-1544367563-12123d8965cd?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Yoga") },
                    new TourActivity { Title = "Snorkeling in Nusa Penida", Description = "Swim with manta rays in crystal clear waters.", Image = new ImageUrl("https://images.unsplash.com/photo-1544551763-46a013bb70d5?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Snorkeling") }
                }
            };
            baliTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier { Name = "Relax", Description = "All-inclusive resort stay", Price = new Money(1200, "USD"), Schedules = generateSchedules(DateTime.UtcNow.AddDays(25), 2, baliTour) }
            };
            context.Tours.Add(baliTour);
        }
        else if (baliTour.Images == null || !baliTour.Images.Any())
        {
            baliTour.Images = new List<TourImage>
            {
                new TourImage { Url = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Bali Rice Field" },
                new TourImage { Url = "https://images.unsplash.com/photo-1518548419970-58e3b4079ab2?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Bali Beach" }
            };
        }

        // ==========================================
        // 5. Rome Tour
        // ==========================================
        var romeTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "Eternal City Explorer");
        if (romeTour == null)
        {
            romeTour = new Tour
            {
                Title = "Eternal City Explorer",
                Description = "Walk through history in the heart of Rome, taking in the Colosseum and Vatican.",
                CategoryId = categoryId,
                DurationDays = 5,
                DurationNights = 4,
                BestTimeToVisit = "October to April.",
                Address = new Address("Via dei Fori Imperiali", "Rome", "Lazio", "Italy", "00184", new Point(new Coordinate(12.4922, 41.8902)) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1552832230-c0197dd311b5?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Colosseum"),
                Highlights = new List<string> { "Colosseum", "Roman Forum", "Vatican City" },
                Tags = new List<string> { "History", "Food", "Art" },
                Recommended = true,
                Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1529260830199-42c42dda5f3d?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Trevi Fountain" }
                },
                Activities = new List<TourActivity>
                {
                    new TourActivity { Title = "Pasta Making Class", Description = "Learn the secrets of Italian pasta from a nonna.", Image = new ImageUrl("https://images.unsplash.com/photo-1556910103-1c02745a30bf?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Pasta") }
                }
            };
            romeTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier { Name = "History Buff", Description = "Direct entry to all museums", Price = new Money(900, "EUR"), Schedules = generateSchedules(DateTime.UtcNow.AddDays(8), 4, romeTour) }
            };
            context.Tours.Add(romeTour);
        }
        else if (romeTour.Images == null || !romeTour.Images.Any())
        {
             romeTour.Images = new List<TourImage>
             {
                 new TourImage { Url = "https://images.unsplash.com/photo-1529260830199-42c42dda5f3d?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Trevi Fountain" }
             };
        }

        // ==========================================
        // 6. Santorini Tour
        // ==========================================
        var santoriniTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "Santorini Sunset Dream");
        if (santoriniTour == null)
        {
            santoriniTour = new Tour
            {
                Title = "Santorini Sunset Dream",
                Description = "Whitewashed houses, blue domes, and the world's most beautiful sunsets.",
                CategoryId = categoryId,
                DurationDays = 5,
                DurationNights = 4,
                BestTimeToVisit = "May to October.",
                Address = new Address("Oia", "Santorini", "Cyclades", "Greece", "84702", new Point(new Coordinate(25.3753, 36.4618)) { SRID = 4326 }),
                MainImage = new ImageUrl("https://images.unsplash.com/photo-1613395877344-13d4c79e4284?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", "Santorini"),
                Highlights = new List<string> { "Oia Sunset", "Red Beach", "Volcano Tour" },
                Tags = new List<string> { "Romance", "Beach", "Views" },
                Recommended = true,
                Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Santorini Dome" }
                },
                Activities = new List<TourActivity>
                {
                    new TourActivity { Title = "Catamaran Cruise", Description = "Sail around the caldera with a BBQ dinner.", Image = new ImageUrl("https://images.unsplash.com/photo-1502444330042-d1a1ddf9bb5b?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=60", "Catamaran") }
                }
            };
            santoriniTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier { Name = "Honeymoon", Description = "Private suite with plunge pool", Price = new Money(2500, "EUR"), Schedules = generateSchedules(DateTime.UtcNow.AddDays(40), 1, santoriniTour) }
            };
            context.Tours.Add(santoriniTour);
        }
        else if (santoriniTour.Images == null || !santoriniTour.Images.Any())
        {
             santoriniTour.Images = new List<TourImage>
             {
                 new TourImage { Url = "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "Santorini Dome" }
             };
        }

        // 7. London Tour (Existing - ensure check)
        var londonTour = await context.Tours.Include(t => t.Images).FirstOrDefaultAsync(t => t.Title == "London Royal Experience");
        if (londonTour == null)
        {
            londonTour = new Tour
            {
                Title = "London Royal Experience",
                Description = "Explore Buckingham Palace, the Tower of London, and enjoy a traditional afternoon tea.",
                CategoryId = categoryId,
                DurationDays = 5,
                DurationNights = 4,
                BestTimeToVisit = "May to September.",
                Address = new Address(
                    "Westminster",
                    "London",
                    "Greater London",
                    "United Kingdom",
                    "SW1A 1AA",
                    new Point(new Coordinate(-0.1419, 51.5014)) { SRID = 4326 }
                ),
                MainImage = new ImageUrl("https://example.com/london.jpg", "Buckingham Palace"),
                Highlights = new List<string> { "Buckingham Palace", "Tower of London", "Afternoon Tea" },
                Tags = new List<string> { "Royal", "History", "Culture" },
                Recommended = true,
                Images = new List<TourImage>
                {
                    new TourImage { Url = "https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "London Eye" }
                }
            };

            londonTour.PriceTiers = new List<TourPriceTier>
            {
                new TourPriceTier
                {
                    Name = "Adult",
                    Description = "Adult Ticket",
                    Price = new Money(120, "GBP"),
                    Schedules = generateSchedules(DateTime.UtcNow.AddDays(5), 5, londonTour)
                }
            };
            context.Tours.Add(londonTour);
        }
        else if (londonTour.Images == null || !londonTour.Images.Any())
        {
             londonTour.Images = new List<TourImage>
             {
                 new TourImage { Url = "https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", AltText = "London Eye" }
             };
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedHotelsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Hotels.AnyAsync()) return;

        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Name = "Grand Plaza Hotel",
                Description = "Luxury stay in the heart of New York.",
                CategoryId = categoryId,
                Address = new Address("5th Avenue", "New York", "NY", "USA", "10001", new Point(new Coordinate(-73.9851, 40.7588)){ SRID = 4326 }),
                MainImage = new ImageUrl("https://example.com/grandplaza.jpg", "Grand Plaza Hotel"),
                StarRating = new StarRating(5),
                ContactInfo = new ContactInfo(new EmailAddress("info@grandplaza.com"), new PhoneNumber("+12125550199"), new Url("https://grandplaza.com")),
                Amenities = new List<string> { "Pool", "Spa", "Gym", "WiFi" },
                Rooms = new List<Room>
                {
                     new Room { RoomNumber = "101", RoomType = "Standard King", BasePrice = new Money(300, "USD"), MaxGuests = 2 },
                     new Room { RoomNumber = "102", RoomType = "Double Queen", BasePrice = new Money(350, "USD"), MaxGuests = 4 }
                }
            },
            new Hotel
            {
                Name = "Seaside Resort",
                Description = "Relax by the beach.",
                CategoryId = categoryId,
                Address = new Address("Ocean Drive", "Miami", "FL", "USA", "33139", new Point(new Coordinate(-80.1300, 25.7906)){ SRID = 4326 }),
                MainImage = new ImageUrl("https://example.com/seaside.jpg", "Seaside Resort"),
                StarRating = new StarRating(4),
                Amenities = new List<string> { "Beach Access", "Pool", "Bar" },
                 Rooms = new List<Room>
                {
                     new Room { RoomNumber = "201", RoomType = "Ocean View", BasePrice = new Money(400, "USD"), MaxGuests = 2 }
                }
            }
        };

        context.Hotels.AddRange(hotels);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCarsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Cars.AnyAsync()) return;

        var toyota = new CarBrand { Name = "Toyota", Logo = "toyota_logo.png" };
        var tesla = new CarBrand { Name = "Tesla", Logo = "tesla_logo.png" };
        context.CarBrands.AddRange(toyota, tesla);
        await context.SaveChangesAsync();

        var cars = new List<Car>
        {
            new Car
            {
                BrandId = toyota.Id,
                Make = "Toyota",
                Model = "Camry",
                CarType = CarCategory.Sedan,
                SeatsCount = 5,
                FuelType = FuelType.Petrol,
                Transmission = TransmissionType.Automatic,
                CategoryId = categoryId,
                Location = new Point(new Coordinate(-118.2437, 34.0522)) { SRID = 4326 }, // LA
                PricingTiers = new List<CarPricingTier>
                {
                    new CarPricingTier { FromHours = 1, ToHours = 24, PricePerHour = new Money(10, "USD") }
                },
                Images = new List<ImageUrl> { new ImageUrl("https://example.com/camry.jpg", "Toyota Camry") }
            },
            new Car
            {
                BrandId = tesla.Id,
                Make = "Tesla",
                Model = "Model 3",
                CarType = CarCategory.Sedan, // Assuming Sedan maps to Luxury/Electric conceptually if strict enum doesn't exist
                SeatsCount = 5,
                FuelType = FuelType.Electric,
                Transmission = TransmissionType.Automatic,
                CategoryId = categoryId,
                Location = new Point(new Coordinate(-122.4194, 37.7749)) { SRID = 4326 }, // SF,
                PricingTiers = new List<CarPricingTier>
                {
                    new CarPricingTier { FromHours = 1, ToHours = 24, PricePerHour = new Money(15, "USD") }
                }
            }
        };

        context.Cars.AddRange(cars);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFlightsAsync(OnlineTravelDbContext context, Guid categoryId)
    {
        if (await context.Flights.AnyAsync()) return;

        var americanAirlines = new Carrier { Name = "American Airlines", Code = new IataCode("AA"), IsActive = true };
        var britishAirways = new Carrier { Name = "British Airways", Code = new IataCode("BA"), IsActive = true };
        context.Carriers.AddRange(americanAirlines, britishAirways);

        var jfk = new Airport { Name = "John F. Kennedy International Airport", Code = new IataCode("JFK"), Address = new Address("Queens", "New York", "NY", "USA", "11430") };
        var lhr = new Airport { Name = "Heathrow Airport", Code = new IataCode("LHR"), Address = new Address("Hillingdon", "London", "UK", "UK", "TW6") };
        var lax = new Airport { Name = "Los Angeles International Airport", Code = new IataCode("LAX"), Address = new Address("Los Angeles", "Los Angeles", "CA", "USA", "90045") };
        context.Airports.AddRange(jfk, lhr, lax);
        
        await context.SaveChangesAsync();

        var flights = new List<Flight>
        {
            new Flight
            {
                FlightNumber = new FlightNumber("AA100"),
                CarrierId = americanAirlines.Id,
                OriginAirportId = jfk.Id,
                DestinationAirportId = lhr.Id,
                Schedule = new DateTimeRange(DateTime.UtcNow.AddDays(30), DateTime.UtcNow.AddDays(30).AddHours(7)),
                CategoryId = categoryId,
                Status = FlightStatus.Scheduled,
                BaggageRules = new List<string> { "23kg Check-in", "7kg Cabin" }
            },
            new Flight
            {
                FlightNumber = new FlightNumber("BA202"),
                CarrierId = britishAirways.Id,
                OriginAirportId = lhr.Id,
                DestinationAirportId = jfk.Id,
                Schedule = new DateTimeRange(DateTime.UtcNow.AddDays(35), DateTime.UtcNow.AddDays(35).AddHours(7).AddMinutes(30)),
                CategoryId = categoryId,
                Status = FlightStatus.Scheduled
            },
             new Flight
            {
                FlightNumber = new FlightNumber("AA45"),
                CarrierId = americanAirlines.Id,
                OriginAirportId = jfk.Id,
                DestinationAirportId = lax.Id,
                Schedule = new DateTimeRange(DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(15).AddHours(6)),
                CategoryId = categoryId,
                Status = FlightStatus.Scheduled
            }
        };

        context.Flights.AddRange(flights);
        await context.SaveChangesAsync();

        // Seed Seats & Fares for these flights
        var allFlights = await context.Flights.ToListAsync();
        var seats = new List<FlightSeat>();
        var fares = new List<FlightFare>();

        foreach (var flight in allFlights)
        {
            // Economy Seats
            seats.Add(new FlightSeat { SeatLabel = "1A", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "1B", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "1C", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "2A", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "2B", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "2C", CabinClass = CabinClass.Economy, IsAvailable = true, FlightId = flight.Id });

            // Business Seats
            seats.Add(new FlightSeat { SeatLabel = "1F", CabinClass = CabinClass.Business, IsAvailable = true, FlightId = flight.Id });
            seats.Add(new FlightSeat { SeatLabel = "1D", CabinClass = CabinClass.Business, IsAvailable = true, FlightId = flight.Id });

            // Seed ONE Fare per flight (Simplification due to strategy limitation)
             fares.Add(new FlightFare { FlightId = flight.Id, BasePrice = new Money(150, "USD"), SeatsAvailable = 100 });
        }

        context.FlightSeats.AddRange(seats);
        context.Set<FlightFare>().AddRange(fares);
        await context.SaveChangesAsync();
    }

    private static List<TourSchedule> generateSchedules(DateTime startDate, int count, Tour tour, int capacity = 30)
    {
        var schedules = new List<TourSchedule>();
        for (int i = 0; i < count; i++)
        {
            schedules.Add(new TourSchedule
            {
                DateRange = new DateRange(DateOnly.FromDateTime(startDate.AddDays(i)), DateOnly.FromDateTime(startDate.AddDays(i))),
                TotalCapacity = capacity,
                AvailableSlots = capacity,
                Status = TourScheduleStatus.Active,
                Tour = tour
            });
        }
        return schedules;
    }
}
