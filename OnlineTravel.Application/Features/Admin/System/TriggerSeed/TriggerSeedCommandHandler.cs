using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Admin.System.TriggerSeed;

public class TriggerSeedCommandHandler : IRequestHandler<TriggerSeedCommand, Result<bool>>
{
    private readonly IDatabaseSeeder _databaseSeeder;

    public TriggerSeedCommandHandler(IDatabaseSeeder databaseSeeder)
    {
        _databaseSeeder = databaseSeeder;
    }

    public async Task<Result<bool>> Handle(TriggerSeedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _databaseSeeder.SeedAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Seeding failed: {ex.Message}");
        }
    }
}
