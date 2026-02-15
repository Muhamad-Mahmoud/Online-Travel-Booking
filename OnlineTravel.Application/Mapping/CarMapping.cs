using Mapster;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Mapping
{
    public class CarMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // DateTimeRange <-> DateTimeRangeDto
            config.NewConfig<DateTimeRange, DateTimeRangeDto>();
            config.NewConfig<DateTimeRangeDto, DateTimeRange>()
                .ConstructUsing(src => new DateTimeRange(src.Start, src.End));

            // ImageUrl <-> ImageUrlDto
            config.NewConfig<ImageUrl, ImageUrlDto>();
            config.NewConfig<ImageUrlDto, ImageUrl>()
                .ConstructUsing(src => new ImageUrl(src.Url, src.AltText));

            // LocationDto -> Point
            config.NewConfig<LocationDto, Point>()
                .ConstructUsing(src => new Point(src.Longitude, src.Latitude) { SRID = 4326 });

            // Point -> LocationDto (للاستعلامات)
            config.NewConfig<Point, LocationDto>()
                .Map(dest => dest.Latitude, src => src.Y)
                .Map(dest => dest.Longitude, src => src.X);

            // Car -> CarDto
            config.NewConfig<Car, CarDto>()
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
            // تم حذف السطر الخاطئ .Ignore(dest => dest.Car)

            // UpdateCarRequest -> Car
            config.NewConfig<UpdateCarRequest, Car>()
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.DeletedAt)
                .Ignore(dest => dest.Brand)
                .Ignore(dest => dest.Category)
                .Ignore(dest => dest.PricingTiers)
                .Ignore(dest => dest.RowVersion);

            // Car -> CarSummaryDto
            // Car -> CarSummaryDto
            config.NewConfig<Car, CarSummaryDto>()
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

            // Car -> CarDetailsDto
            config.NewConfig<Car, CarDetailsDto>()
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
            // تحويل CarPricingTier -> CarPricingTierDto (إذا لم يكن موجوداً)
            config.NewConfig<CarPricingTier, CarPricingTierDto>()
                .Map(dest => dest.PricePerHour, src => src.PricePerHour); // Money -> MoneyDto تلقائي
        
    }
    }
}
