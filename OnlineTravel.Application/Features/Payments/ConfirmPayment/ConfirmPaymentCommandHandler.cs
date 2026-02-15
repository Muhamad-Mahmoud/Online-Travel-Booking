using MediatR;
using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Payments;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Payments.ConfirmPayment;

public sealed class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmPaymentCommandHandler> _logger;

    public ConfirmPaymentCommandHandler(IUnitOfWork unitOfWork, ILogger<ConfirmPaymentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing payment confirmation for Booking {BookingId}", request.BookingId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var booking = await _unitOfWork.Repository<BookingEntity>().GetByIdAsync(request.BookingId);

            if (booking == null)
            {
                _logger.LogWarning("Payment confirmation failed: Booking {BookingId} not found", request.BookingId);
                return Result.Failure(Error.NotFound($"Booking {request.BookingId} was not found."));
            }

            // If already paid or confirmed, return success
            if (booking.PaymentStatus == PaymentStatus.Paid || booking.Status == BookingStatus.Confirmed)
            {
                _logger.LogInformation("Booking {BookingId} is already paid/confirmed. Skipping.", request.BookingId);
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Success();
            }

            // WEBHOOK DEDUPLICATION
            if (!string.IsNullOrEmpty(request.EventId))
            {
                var existingEvent = await _unitOfWork.Repository<ProcessedWebhookEvent>()
                    .FindAsync(e => e.EventId == request.EventId);

                if (existingEvent != null)
                {
                    _logger.LogInformation("Event {EventId} was already processed. Skipping.", request.EventId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Success();
                }

                var webhookEvent = ProcessedWebhookEvent.Create(request.EventId, "Stripe");
                await _unitOfWork.Repository<ProcessedWebhookEvent>().AddAsync(webhookEvent);
            }

            // Confirm Payment (Updates status and payment status)
            booking.ConfirmPayment(request.PaymentIntentId);

            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Booking {BookingId} confirmed successfully via payment confirmation", request.BookingId);
            return Result.Success();
        }
        catch (DomainException ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogWarning("Payment confirmation rejected for Booking {BookingId}: {Message}", request.BookingId, ex.Message);
            return Result.Failure(Error.Validation(ex.Message));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Unexpected error during payment confirmation for Booking {BookingId}", request.BookingId);
            throw;
        }
    }
}
