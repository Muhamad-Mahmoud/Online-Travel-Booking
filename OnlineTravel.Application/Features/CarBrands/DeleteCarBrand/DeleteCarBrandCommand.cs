using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.DeleteCarBrand;

public sealed record DeleteCarBrandCommand(Guid Id) : IRequest<Result<bool>>;
