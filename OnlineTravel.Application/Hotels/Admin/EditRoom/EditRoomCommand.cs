using MediatR;
using OnlineTravel.Application.Hotels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Admin.EditRoom
{
    public class EditRoomCommand : IRequest<Result<EditRoomResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePricePerNight { get; set; }
    }

}
