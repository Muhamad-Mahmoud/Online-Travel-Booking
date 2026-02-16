using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.EditRoom
{
    public class EditRoomCommandValidator : AbstractValidator<EditRoomCommand>
    {
        public EditRoomCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Room ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room name is required")
                .MaximumLength(200).WithMessage("Room name must not exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.BasePricePerNight)
                .GreaterThan(0).WithMessage("Base price must be greater than zero");
        }
    }

}
