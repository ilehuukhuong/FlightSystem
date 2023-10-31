using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IFlightRepository
    {
        void CreateFlight(UpsertFlightDto flightDto);
        bool DeleteFlight(int id);
        void UpdateFlight(UpsertFlightDto updateFlightDto);
        Task<Flight> GetFlightAsync(int id);
        Task<PagedList<Flight>> GetFlightsAsync(PaginationParams paginationParams);
    }
}
