using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FlightRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void CreateFlight(UpsertFlightDto flight)
        {
            _context.Flights.Add(_mapper.Map<Flight>(flight));
        }

        public bool DeleteFlight(int id)
        {
            var flight = _context.Flights.Where(x => x.Id == id).SingleOrDefault();

            if (flight == null)
            {
                return false;
            }

            _context.Flights.Remove(flight);

            return true;
        }

        public async Task<Flight> GetFlightAsync(int id)
        {
            return await _context.Flights.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<Flight>> GetFlightsAsync(PaginationParams paginationParams)
        {
            var query = _context.Flights.AsQueryable();

            return await PagedList<Flight>.CreateAsync(query.AsNoTracking().ProjectTo<Flight>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public void UpdateFlight(UpsertFlightDto flight)
        {
            _mapper.Map(flight, _context.Flights.Where(x => x.Id == flight.Id).FirstOrDefault());
        }
    }
}
