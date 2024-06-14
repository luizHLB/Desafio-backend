using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Plan")]
    [AllowAnonymous]
    public class PlanController : Controller
    {
        private readonly IPlanService _service;
        public PlanController(IPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _service.GetPlans());
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }
    }
}
