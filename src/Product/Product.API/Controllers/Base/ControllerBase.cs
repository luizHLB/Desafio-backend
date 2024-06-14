using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers.Base
{
    [ApiController]
    public class BaseController : Controller
    {
        public IBaseServiceUserHandler Service { get; set; }

        public BaseController(IBaseServiceUserHandler service)
        {
            Service = service;
        }
    }
}
