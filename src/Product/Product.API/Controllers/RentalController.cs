using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
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
        public async Task<IActionResult> Get([FromQuery] long? driverId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _service.PagedListAsync(driverId, page, pageSize));
        }

        [HttpGet("{id}:long")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            return Ok(await _service.GetDtoById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalDTO dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] UpdateRentalDTO dto)
        {
            return Ok(await _service.Complete(dto));
        }
    }
}
