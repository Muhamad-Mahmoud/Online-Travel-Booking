using MediatR;
using OnlineTravel.Application.Hotels.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Hotels.Admin.ConfigureSeasonalPricing
{
    public class ConfigureSeasonalPricingCommandHandler : IRequestHandler<ConfigureSeasonalPricingCommand, Result<ConfigureSeasonalPricingResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfigureSeasonalPricingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ConfigureSeasonalPricingResponse>> Handle(ConfigureSeasonalPricingCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetWithSeasonalPricesAsync(request.RoomId);
            if (room == null)
                return Result<ConfigureSeasonalPricingResponse>.Failure("Room not found");

            var dateRange = new DateRange(request.StartDate, request.EndDate);
            var pricePerNight = new Money(request.Price, "USD");
            var seasonalPrice = new SeasonalPrice(room.Id, dateRange, pricePerNight);
            room.AddSeasonalPrice(seasonalPrice);

            await _unitOfWork.Repository<SeasonalPrice>().AddAsync(seasonalPrice, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ConfigureSeasonalPricingResponse>.Success(new ConfigureSeasonalPricingResponse
            {
                RoomId = room.Id,
                Message = $"Seasonal pricing configured for period {request.StartDate:yyyy-MM-dd} to {request.EndDate:yyyy-MM-dd}"
            });
        }
    }
}
