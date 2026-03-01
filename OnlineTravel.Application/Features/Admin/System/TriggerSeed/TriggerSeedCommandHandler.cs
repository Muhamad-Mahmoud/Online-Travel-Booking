using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Admin.System.TriggerSeed
{
	public class TriggerSeedCommandHandler : IRequestHandler<TriggerSeedCommand, Result<bool>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public TriggerSeedCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<bool>> Handle(TriggerSeedCommand request, CancellationToken cancellationToken)
		{
			// Logic to trigger seed
			await Task.CompletedTask;
			return Result<bool>.Success(true);
		}
	}
}

