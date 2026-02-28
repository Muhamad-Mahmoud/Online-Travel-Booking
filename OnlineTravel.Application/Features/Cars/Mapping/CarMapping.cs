using Mapster;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Cars.Shared;
using OnlineTravel.Application.Features.Cars.UpdateCar;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;


namespace OnlineTravel.Application.Features.Cars.Mapping
{
	public class CarMapping : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// DateTimeRange <-> DateTimeRangeResponse
			config.NewConfig<DateTimeRange, DateTimeRangeResponse>();
			config.NewConfig<DateTimeRangeResponse, DateTimeRange>()
				.ConstructUsing(src => new DateTimeRange(src.Start, src.End));

			// ImageUrl <-> ImageUrlResponse
			config.NewConfig<ImageUrl, ImageUrlResponse>();
			config.NewConfig<ImageUrlResponse, ImageUrl>()
				.ConstructUsing(src => new ImageUrl(src.Url, src.AltText));

			// LocationResponse -> Point
			config.NewConfig<LocationResponse, Point>()
				.ConstructUsing(src => new Point(src.Longitude, src.Latitude) { SRID = 4326 });

			// Point -> LocationResponse
			config.NewConfig<Point, LocationResponse>()
				.Map(dest => dest.Latitude, src => src.Y)
				.Map(dest => dest.Longitude, src => src.X);


			// Car -> GetCarByIdResponse
			config.NewConfig<Car, GetCarByIdResponse>()
				.Map(dest => dest.BrandName, src => src.Brand.Name)
				.Map(dest => dest.CategoryTitle, src => src.Category.Title)
				.Map(dest => dest.AvailableDates, src => src.AvailableDates)
				.Map(dest => dest.Images, src => src.Images)
				.Map(dest => dest.Location, src => src.Location);

			// CreateCarRequest -> Car
			config.NewConfig<CreateCarRequest, Car>()
				.Ignore(dest => dest.Id)
				.Ignore(dest => dest.CreatedAt)
				.Ignore(dest => dest.UpdatedAt)
				.Ignore(dest => dest.DeletedAt)
				.Ignore(dest => dest.Brand)
				.Ignore(dest => dest.Category)
				.Ignore(dest => dest.PricingTiers)
				.Ignore(dest => dest.RowVersion);

			// UpdateCarRequest -> Car
			config.NewConfig<UpdateCarRequest, Car>()
				.Ignore(dest => dest.CreatedAt)
				.Ignore(dest => dest.UpdatedAt)
				.Ignore(dest => dest.DeletedAt)
				.Ignore(dest => dest.Brand)
				.Ignore(dest => dest.Category)
				.Ignore(dest => dest.PricingTiers)
				.Ignore(dest => dest.RowVersion);

			// Car -> CarSummaryResponse
			config.NewConfig<Car, CarSummaryResponse>()
				.Map(dest => dest.BrandName, src => src.Brand == null ? null : src.Brand.Name)
				.Map(dest => dest.CategoryTitle, src => src.Category == null ? null : src.Category.Title)
				.Map(dest => dest.MainImage, src => src.Images == null || !src.Images.Any() ? null : src.Images.First().Url)
				.Map(dest => dest.PricePerHour, src =>
					src.PricingTiers == null || !src.PricingTiers.Any() ? 0 :
					src.PricingTiers.OrderBy(t => t.FromHours).First().PricePerHour == null ? 0 :
					src.PricingTiers.OrderBy(t => t.FromHours).First().PricePerHour.Amount)
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Make, src => src.Make)
				.Map(dest => dest.Model, src => src.Model)
				.Map(dest => dest.CarType, src => src.CarType)
				.Map(dest => dest.SeatsCount, src => src.SeatsCount)
				.Map(dest => dest.FuelType, src => src.FuelType)
				.Map(dest => dest.Transmission, src => src.Transmission);

			// Car -> CarDetailsResponse
			config.NewConfig<Car, CarDetailsResponse>()
				.Map(dest => dest.BrandName, src => src.Brand == null ? null : src.Brand.Name)
				.Map(dest => dest.CategoryTitle, src => src.Category == null ? null : src.Category.Title)
				.Map(dest => dest.MainImage, src => src.Images == null || !src.Images.Any() ? null : src.Images.First().Url)
				.Map(dest => dest.PricePerHour, src =>
					src.PricingTiers == null || !src.PricingTiers.Any() ? 0 :
					src.PricingTiers.OrderBy(t => t.FromHours).First().PricePerHour == null ? 0 :
					src.PricingTiers.OrderBy(t => t.FromHours).First().PricePerHour.Amount)
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Make, src => src.Make)
				.Map(dest => dest.Model, src => src.Model)
				.Map(dest => dest.CarType, src => src.CarType)
				.Map(dest => dest.SeatsCount, src => src.SeatsCount)
				.Map(dest => dest.FuelType, src => src.FuelType)
				.Map(dest => dest.Transmission, src => src.Transmission)
				.Map(dest => dest.Features, src => src.Features)
				.Map(dest => dest.AvailableDates, src => src.AvailableDates)
				.Map(dest => dest.CancellationPolicy, src => src.CancellationPolicy)
				.Map(dest => dest.Location, src => src.Location)
				.Map(dest => dest.Images, src => src.Images)
				.Map(dest => dest.PricingTiers, src => src.PricingTiers)
				.Map(dest => dest.CreatedAt, src => src.CreatedAt)
				.Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
		}
	}
}
