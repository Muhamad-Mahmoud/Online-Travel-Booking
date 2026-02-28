using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Admin.System.TriggerSeed
{
	public class TriggerSeedCommandHandler : IRequestHandler<TriggerSeedCommand, OnlineTravel.Application.Common.Result<bool>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public TriggerSeedCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<OnlineTravel.Application.Common.Result<bool>> Handle(TriggerSeedCommand request, CancellationToken cancellationToken)
		{
			// Logic to trigger seed
			await Task.CompletedTask;
			return OnlineTravel.Application.Common.Result<bool>.Success(true);
		}
	}
}

