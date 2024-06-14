using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.DTO.Authentication;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Authentication")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _service;
        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthenticationDTO dto)
        {
            try
            {
                return Ok(await _service.Authenticate(dto));
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }

}
