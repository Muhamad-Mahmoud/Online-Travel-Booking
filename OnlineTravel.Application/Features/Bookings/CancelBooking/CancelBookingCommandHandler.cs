using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.Bookings.Specifications.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;
using Microsoft.Extensions.Logging;

using OnlineTravel.Application.Features.Bookings.Shared.DTOs;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking;

public sealed class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<CancelBookingResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelBookingCommandHandler> _logger;

    public CancelBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CancelBookingResponse>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to cancel booking {BookingId} for User {UserId}", request.BookingId, request.UserId);

        var spec = new GetBookingByIdSpec(request.BookingId);
        var booking = await _unitOfWork.Repository<BookingEntity>().GetEntityWithAsync(spec, cancellationToken);

        if (booking == null)
        {
            _logger.LogWarning("Cancel failed: Booking {BookingId} not found", request.BookingId);
            return Result<CancelBookingResponse>.Failure(Error.NotFound($"Booking {request.BookingId} Not Found"));
        }

        if (booking.UserId != request.UserId)
        {
            _logger.LogWarning("Cancel failed: Unauthorized attempt by User {UserId} to cancel Booking {BookingId}", request.UserId, request.BookingId);
            return Result<CancelBookingResponse>.Failure(Error.Validation("You are not authorized to cancel this booking."));
        }

        try
        {
            booking.Cancel();

            _unitOfWork.Repository<BookingEntity>().Update(booking);

            await _unitOfWork.Complete();
            _logger.LogInformation("Booking {BookingId} cancelled successfully", request.BookingId);

            return Result<CancelBookingResponse>.Success(new CancelBookingResponse(booking.BookingReference.Value, booking.Status.ToString()));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Cancel failed for Booking {BookingId}: {Message}", request.BookingId, ex.Message);
            return Result<CancelBookingResponse>.Failure(Error.Conflict(ex.Message));
        }
        catch (DbUpdateConcurrencyException ex)
        {
             _logger.LogError(ex, "Concurrency error canceling Booking {BookingId}", request.BookingId);
            return Result<CancelBookingResponse>.Failure(Error.Validation("The item is no longer available. Someone else might have booked it just now. Please try again."));
        }

    }
}
