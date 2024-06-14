using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO.Driver;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Driver")]
    [TokenHandler]
    public class DriverController : BaseController<Driver, DriverDTO>
    {
        private readonly IDriverService _service;
        public DriverController(IDriverService service) : base (service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = "", [FromQuery] string cnpj = "", [FromQuery] string cnh = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.PagedListAsync(name, cnpj, cnh, page, pageSize));
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}:long")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            try
            {
                return Ok(await _service.GetDtoById(id));
            }
            catch (RecordNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateDriverDTO dto)
        {
            try
            {
                return Ok(await _service.Create(dto));
            }
            catch (EntityConstraintException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpPatch("{id:long}")]
        public async Task<IActionResult> Patch([FromRoute] long id, [FromForm] UpdateDriverDTO dto)
        {
            try
            {
                return Ok(await _service.Update(id, dto));
            }
            catch (EntityConstraintException e)
            {
                return BadRequest(e.Message);
            }
            catch (RecordNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }
    }
}
