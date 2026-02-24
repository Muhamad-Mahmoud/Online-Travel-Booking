using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;

public sealed record UpdateCarBrandCommand(Guid Id, UpdateCarBrandRequest Data) : IRequest<Result<bool>>;
