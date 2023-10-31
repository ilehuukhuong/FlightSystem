using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PermissionController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public PermissionController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<PagedList<PermissionDto>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var permissions = await _uow.PermissionRepository.GetPermissionsAsync(paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(permissions.CurrentPage, permissions.PageSize, permissions.TotalCount, permissions.TotalPages));

            return Ok(permissions);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Create(PermissionDto permissionDto)
        {        
            _uow.PermissionRepository.CreatePermission(permissionDto, await _uow.UserRepository.GetUserByIdAsync(User.GetUserId()));

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Update(UpdatePermissionDto updatePermissionDto)
        {
            _uow.PermissionRepository.UpdatePermission(updatePermissionDto);

            if (await _uow.Complete()) return Ok("");

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_uow.PermissionRepository.DeletePermission(id) == false) return BadRequest("This permission is being used or Not Found");

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }
    }
}
