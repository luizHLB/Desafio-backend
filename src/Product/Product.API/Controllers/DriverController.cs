﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO;
using Product.Domain.DTO.Driver;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [TokenHandler]
    [Route("api/v1/Driver")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DriverController : BaseController
    {
        private readonly IDriverService _service;
        public DriverController(IDriverService service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedListDTO<DriverDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromQuery] string? name = "", [FromQuery] string? cnpj = "", [FromQuery] string? cnh = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _service.PagedListAsync(name, cnpj, cnh, page, pageSize));
        }

        [HttpGet("{id}:long")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DriverDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            return Ok(await _service.GetDtoById(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DriverDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromForm] CreateDriverDTO dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPatch("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DriverDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Patch([FromRoute] long id, [FromForm] UpdateDriverDTO dto)
        {
            return Ok(await _service.Update(id, dto));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DriverDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            await _service.Remove(id);
            return Ok();
        }
    }
}
