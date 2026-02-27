using MediatR;
using OnlineTravel.Application.Features.Categories.Shared.DTOs;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Categories.GetCategoriesByType
{
	public record GetCategoriesByTypeQuery(CategoryType Type) : IRequest<Result<List<CategoryDto>>>;
}
