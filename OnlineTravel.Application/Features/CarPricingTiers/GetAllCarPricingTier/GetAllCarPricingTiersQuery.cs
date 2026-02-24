using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetAll;

public sealed record GetAllCarPricingTiersQuery(Guid? CarId = null) : IRequest<Result<IReadOnlyList<GetAllCarPricingTiersResponse>>>;
