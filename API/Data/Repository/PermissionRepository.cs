using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PermissionRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void CreatePermission(PermissionDto permissionDto, AppUser appUser)
        {
            permissionDto.Creator = _mapper.Map<AppUserDto>(appUser);
            var permission = _mapper.Map<Permission>(permissionDto);
            permission.PermissionUsers = _mapper.Map<List<PermissionUser>>(permissionDto.AppUserDtos);
            permission.Created = DateTime.UtcNow;
            _context.Permissions.Add(permission);
        }

        public bool DeletePermission(int id)
        {
            var permission = _context.Permissions.Where(x => x.Id == id).Include(x => x.ConfigurationPermissions).Include(x => x.PermissionUsers).SingleOrDefault();

            if (permission == null)
            {
                return false;
            }

            if (permission.ConfigurationPermissions.Any())
            {
                return false;
            }

            // Remove associated PermissionUsers
            foreach (var permissionUser in permission.PermissionUsers.ToList())
            {
                _context.Remove(permissionUser);
            }

            _context.Permissions.Remove(permission);

            return true;
        }

        public async Task<PermissionDto> GetPermissionAsync(int id)
        {
            return _mapper.Map<PermissionDto>(await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<PagedList<PermissionDto>> GetPermissionsAsync(PaginationParams paginationParams)
        {
            var query = _context.Permissions.AsQueryable();

            return await PagedList<PermissionDto>.CreateAsync(query.AsNoTracking().ProjectTo<PermissionDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public void UpdatePermission(UpdatePermissionDto updatePermissionDto)
        {
            var permission = _context.Permissions.Where(x => x.Id == updatePermissionDto.Id).Include(x => x.PermissionUsers).SingleOrDefault();
            permission = _mapper.Map(updatePermissionDto, permission);
            permission.PermissionUsers = _mapper.Map<List<PermissionUser>>(updatePermissionDto.AppUserDtos);
            _context.Permissions.Update(permission);
        }
    }
}
