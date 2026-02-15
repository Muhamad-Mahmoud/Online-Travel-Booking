using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Payments;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Infrastructure.Persistence.Context;

/// <summary>
/// Entity Framework Core DbContext for Travel Marketplace.
/// </summary>
public class OnlineTravelDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public OnlineTravelDbContext(DbContextOptions<OnlineTravelDbContext> options)
        : base(options)
    {
    }

    // Core
    public DbSet<Category> Categories => Set<Category>();

    // Tours
    public DbSet<Tour> Tours => Set<Tour>();

    public DbSet<TourSchedule> TourSchedules => Set<TourSchedule>();
    public DbSet<TourActivity> TourActivities => Set<TourActivity>();

    public DbSet<TourPriceTier> TourPriceTiers => Set<TourPriceTier>();

    // Flights
    public DbSet<Carrier> Carriers => Set<Carrier>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FareRule> FareRules => Set<FareRule>();
    public DbSet<FlightFare> FlightFares => Set<FlightFare>();
    public DbSet<FlightSeat> FlightSeats => Set<FlightSeat>();

    // Cars
    public DbSet<CarBrand> CarBrands => Set<CarBrand>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarPricingTier> CarPricingTiers => Set<CarPricingTier>();


    // Hotels
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();

    // Bookings
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<BookingDetail> BookingDetails => Set<BookingDetail>();

    // Payments
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<ProcessedWebhookEvent> ProcessedWebhookEvents => Set<ProcessedWebhookEvent>();

    // Reviews
    public DbSet<Review> Reviews => Set<Review>();

    // Favorites
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineTravelDbContext).Assembly);
    }
}




