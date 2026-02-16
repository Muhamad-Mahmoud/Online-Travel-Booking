using MediatR;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.Commands.AddFavorite;

public sealed record AddFavoriteCommand(
    Guid UserId,
    Guid ItemId
) : IRequest<Result<Guid>>;
