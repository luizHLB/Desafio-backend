using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO.Rental;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [TokenHandler]
    [Route("api/v1/Rental")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RentalController : BaseController
    {
        private readonly IRentalService _service;
        public RentalController(IRentalService service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]long? driverId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.PagedListAsync(driverId, page, pageSize));
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
        public async Task<IActionResult> Create([FromBody] CreateRentalDTO dto)
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

        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] UpdateRentalDTO dto)
        {
            try
            {
                return Ok(await _service.Complete(dto));
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
    }
}
