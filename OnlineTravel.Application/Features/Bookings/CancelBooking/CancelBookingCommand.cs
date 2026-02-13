using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(
    Guid BookingId,
    Guid UserId
    ) : IRequest<Result<Guid>>;