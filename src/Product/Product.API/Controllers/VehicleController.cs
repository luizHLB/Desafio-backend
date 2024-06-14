using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [TokenHandler]
    [Route("api/v1/Vehicle")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VehicleController : BaseController
    {
        private readonly IVehicleService _service;

        public VehicleController(IVehicleService service) : base (service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string licensePlate = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10 )
        {
            try
            {
                return Ok(await _service.PagedListAsync(licensePlate, page, pageSize));
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
        public async Task<IActionResult> Create([FromBody] CreateVehicleDTO dto)
        {
            try
            {
                return Ok(await _service.Create(dto));
            }
            catch(EntityConstraintException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchVehicleDTO dto)
        {
            try
            {
                return Ok(await _service.Update(dto));
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
        public async Task<IActionResult> Delete([FromRoute]long id)
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
