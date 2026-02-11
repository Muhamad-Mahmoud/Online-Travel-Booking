using OnlineTravel.Domain.Entities.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Interfaces.Services
{
    public interface IAirportService
    {
        Task<Airport?> GetAirportById(Guid id);
        Task<IReadOnlyList<Airport>> GetAllAirports();
        Task<Airport> CreateAirport(Airport airport);
        Task<Airport> UpdateAirport(Guid id,Airport airport);
        Task<bool> DeleteAirport(Guid id);
    }
}
