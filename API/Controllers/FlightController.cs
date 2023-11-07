using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FlightController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public FlightController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult<PagedList<Flight>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var flights = await _uow.FlightRepository.GetFlightsAsync(paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(flights.CurrentPage, flights.PageSize, flights.TotalCount, flights.TotalPages));

            return Ok(flights);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult<Flight>> Detail(int id)
        {
            var flight = await _uow.FlightRepository.GetFlightAsync(id);
            if (flight == null) return NotFound();
            return Ok(flight);
        }

        [HttpPost]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Create(UpsertFlightDto flight)
        {        
            _uow.FlightRepository.CreateFlight(flight);

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Update(UpsertFlightDto flight)
        {
            _uow.FlightRepository.UpdateFlight(flight);

            if (await _uow.Complete()) return Ok("");

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireStaffGoRole")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_uow.FlightRepository.DeleteFlight(id) == false) return BadRequest("This flight is being used or Not Found");

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }
    }
}
