using FluentValidation;

namespace OnlineTravel.Application.Features.Hotels.Admin.AddRoom
{
	public class AddRoomCommandValidator : AbstractValidator<AddRoomCommand>
	{
		public AddRoomCommandValidator()
		{
			RuleFor(x => x.HotelId).NotEmpty().WithMessage("Hotel ID is required");
			RuleFor(x => x.RoomNumber).NotEmpty().WithMessage("Room number is required").MaximumLength(50);
			RuleFor(x => x.Name).NotEmpty().WithMessage("Room name is required").MaximumLength(200);
			RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
			RuleFor(x => x.BasePricePerNight).GreaterThan(0).WithMessage("Base price per night must be greater than zero");
		}
	}

}
