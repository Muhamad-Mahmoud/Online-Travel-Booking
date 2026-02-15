using OnlineTravel.Domain.Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<Hotel?> GetBySlugAsync(string slug);
        Task<bool> SlugExistsAsync(string slug);
        Task<Hotel?> GetWithRoomsAsync(Guid id);
        Task<Hotel?> GetWithReviewsAsync(Guid id);
    }
}
