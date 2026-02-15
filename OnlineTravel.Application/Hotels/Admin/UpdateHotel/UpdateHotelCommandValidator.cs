using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Admin.UpdateHotel
{
    public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
    {
        public UpdateHotelCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Hotel ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hotel name is required")
                .MaximumLength(200).WithMessage("Hotel name must not exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required");

            RuleFor(x => x.MainImage)
                .NotEmpty().WithMessage("Main image is required");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("Phone is required");

            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }

}
