using AutoMapper;
using MediatR;
using OnlineTravel.Application.Hotels.Common;
using OnlineTravel.Application.Hotels.Dtos;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Hotels;


namespace OnlineTravel.Application.Hotels.Public.GetHotelDetails
{
    public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, Result<HotelDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<HotelDetailsDto>> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _unitOfWork.Hotels.GetWithRoomsAsync(request.Id);

            if (hotel == null)
                return Result<HotelDetailsDto>.Failure("Hotel not found");

            var dto = _mapper.Map<HotelDetailsDto>(hotel);

            return Result<HotelDetailsDto>.Success(dto);
        }
    }

}
