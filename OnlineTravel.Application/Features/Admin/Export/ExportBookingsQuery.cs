using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Admin.Export;

public record ExportBookingsQuery : IRequest<Result<byte[]>>;
