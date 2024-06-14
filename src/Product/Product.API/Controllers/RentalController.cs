using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.DTO.Rental;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Plan")]
    [AllowAnonymous]
    public class RentalController : Controller
    {
        private readonly IRentalService _service;
        public RentalController(IRentalService service)
        {
            _service = service;
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

        [HttpPost]
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
