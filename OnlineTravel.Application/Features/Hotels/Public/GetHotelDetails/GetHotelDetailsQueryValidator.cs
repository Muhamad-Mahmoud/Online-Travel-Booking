using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
    public class GetHotelDetailsQueryValidator : AbstractValidator<GetHotelDetailsQuery>
    {
        public GetHotelDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Hotel ID is required");
        }
    }

}
