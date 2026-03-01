using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Admin.Export;

public record ExportBookingsQuery : IRequest<Result<byte[]>>;
