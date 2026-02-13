using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.Bookings.CreateBooking;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking;

public sealed class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Microsoft.Extensions.Logging.ILogger<CancelBookingCommandHandler> _logger;

    public CancelBookingCommandHandler(IUnitOfWork unitOfWork, Microsoft.Extensions.Logging.ILogger<CancelBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetBookingByIdSpec(request.BookingId);
        var Booking = await _unitOfWork.Repository<BookingEntity>().GetEntityWithAsync(spec, cancellationToken);

        if (Booking == null)
        {
            return Result<Guid>.Failure(Error.NotFound($"Booking {request.BookingId} Not Found"));
        }

        if (Booking.UserId != request.UserId)
        {
            return Result<Guid>.Failure(Error.Validation("You are not authorized to cancel this booking."));
        }

        Booking!.Cancel();

        _unitOfWork.Repository<BookingEntity>().Update(Booking);

        try
        {
            await _unitOfWork.Complete();
            return Result<Guid>.Success(request.BookingId);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<Guid>.Failure(Error.Validation("The item is no longer available. Someone else might have booked it just now. Please try again."));
        }

    }
}
