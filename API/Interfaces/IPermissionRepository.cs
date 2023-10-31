using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPermissionRepository
    {
        void CreatePermission(PermissionDto permissionDto, AppUser appUser);
        bool DeletePermission(int id);
        void UpdatePermission(UpdatePermissionDto updatePermissionDto);
        Task<PermissionDto> GetPermissionAsync(int id);
        Task<PagedList<PermissionDto>> GetPermissionsAsync(PaginationParams paginationParams);
    }
}
