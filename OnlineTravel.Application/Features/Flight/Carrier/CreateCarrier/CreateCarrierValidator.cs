using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier
{
    public class CreateCarrierValidator : AbstractValidator<CreateCarrierCommand>
    {
        public CreateCarrierValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Code).NotEmpty().Length(2, 3); // Carrier codes are usually 2 or 3 chars
        }

    }
}
