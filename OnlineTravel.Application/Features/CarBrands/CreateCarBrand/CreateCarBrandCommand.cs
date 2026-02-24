using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.CreateCarBrand;

public sealed record CreateCarBrandCommand(CreateCarBrandRequest Data) : IRequest<Result<Guid>>;
