using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Data.Repository
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ConfigurationRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void CreateConfiguration(Configuration configuration)
        {
            _context.Configurations.Add(configuration);
        }

        public bool DeleteConfiguration(int id)
        {
            var configuration = _context.Configurations.Where(x => x.Id == id).Include(x => x.ConfigurationPermissions).SingleOrDefault();

            if (configuration == null)
            {
                return false;
            }

            // Remove associated ConfigurationUsers
            foreach (var configurationPermissions in configuration.ConfigurationPermissions.ToList())
            {
                _context.Remove(configurationPermissions);
            }

            _context.Configurations.Remove(configuration);

            return true;
        }

        public async Task<Configuration> GetConfigurationAsync(int id)
        {
            return await _context.Configurations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<Configuration>> GetConfigurationsAsync(PaginationParams paginationParams)
        {
            var query = _context.Configurations.AsQueryable();

            return await PagedList<Configuration>.CreateAsync(query.AsNoTracking().ProjectTo<Configuration>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public void UpdateConfiguration(Configuration configuration)
        {
            _mapper.Map(configuration, _context.Configurations.Where(x => x.Id == configuration.Id).Include(x => x.ConfigurationPermissions).FirstOrDefault());
        }
    }
}
