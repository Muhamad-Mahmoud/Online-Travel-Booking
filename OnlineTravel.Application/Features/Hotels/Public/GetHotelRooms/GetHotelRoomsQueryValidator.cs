using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms
{
    public class GetHotelRoomsQueryValidator : AbstractValidator<GetHotelRoomsQuery>
    {
        public GetHotelRoomsQueryValidator()
        {
            RuleFor(x => x.HotelId)
                .NotEmpty().WithMessage("Hotel ID is required");

            RuleFor(x => x.CheckOut)
                .GreaterThan(x => x.CheckIn).WithMessage("Check-out must be after check-in")
                .When(x => x.CheckIn.HasValue && x.CheckOut.HasValue);
        }
    }
    
}
