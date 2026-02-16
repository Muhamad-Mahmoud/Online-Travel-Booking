using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability
{
    public class ManageAvailabilityCommandValidator : AbstractValidator<ManageAvailabilityCommand>
    {
        public ManageAvailabilityCommandValidator()
        {
            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("Room ID is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date");
        }
    }

}
