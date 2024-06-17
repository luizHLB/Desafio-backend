using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.DTO.Plan;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [TokenHandler]
    [Route("api/v1/Plan")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlanController : BaseController
    {
        private readonly IPlanService _service;
        public PlanController(IPlanService service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<PlanDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetPlans());
        }
    }
}
