using MediatR;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using System.Collections.Generic;

namespace OnlineTravel.Application.Features.Admin.GetCategoriesByType
{
    public record GetCategoriesByTypeQuery(CategoryType Type) : IRequest<Result<List<CategoryDto>>>;
}
