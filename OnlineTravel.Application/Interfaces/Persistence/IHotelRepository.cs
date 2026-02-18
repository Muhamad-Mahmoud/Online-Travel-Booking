using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<bool> SlugExistsAsync(string slug);
        Task<Hotel?> GetWithRoomsAsync(Guid id);
        Task<Hotel?> GetWithReviewsAsync(Guid id);
    }
}
