using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers.Base;
using Product.API.Filter;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [TokenHandler]
    [Produces("application/json")]
    [Route("api/v1/Notification")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.PagedListAsync(page, pageSize));
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: 500);
            }
        }
    }
}
