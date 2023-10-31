using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IConfigurationRepository
    {
        void CreateConfiguration(Configuration configurationDto);
        bool DeleteConfiguration(int id);
        void UpdateConfiguration(Configuration updateConfigurationDto);
        Task<Configuration> GetConfigurationAsync(int id);
        Task<PagedList<Configuration>> GetConfigurationsAsync(PaginationParams paginationParams);
    }
}
