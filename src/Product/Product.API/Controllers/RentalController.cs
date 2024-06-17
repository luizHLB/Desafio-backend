using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO;
using Product.Domain.DTO.Rental;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedListDTO<RentalDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromQuery] long? driverId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _service.PagedListAsync(driverId, page, pageSize));
        }

        [HttpGet("{id}:long")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            return Ok(await _service.GetDtoById(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateRentalDTO dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPost("complete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Complete([FromBody] UpdateRentalDTO dto)
        {
            return Ok(await _service.Complete(dto));
        }
    }
}
