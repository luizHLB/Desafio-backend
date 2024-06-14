using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers.Base
{
    [ApiController]
    public class BaseController : Controller
    {
        public IBaseUserHandler Service { get; set; }

        public BaseController(IBaseUserHandler service)
        {
            Service = service;
        }
    }
}
