using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ConfigurationController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ConfigurationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult<PagedList<Configuration>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var configurations = await _uow.ConfigurationRepository.GetConfigurationsAsync(paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(configurations.CurrentPage, configurations.PageSize, configurations.TotalCount, configurations.TotalPages));

            return Ok(configurations);
        }

        [HttpPost]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Create(Configuration configuration)
        {        
            _uow.ConfigurationRepository.CreateConfiguration(configuration);

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Update(Configuration configuration)
        {
            _uow.ConfigurationRepository.UpdateConfiguration(configuration);

            if (await _uow.Complete()) return Ok("");

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_uow.ConfigurationRepository.DeleteConfiguration(id) == false) return BadRequest("This configuration is being used or Not Found");

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }
    }
}
