using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces.Services;

namespace Product.API.Controllers.Base
{
    public class BaseController<T, TT> : Controller where T : class where TT : class
    {
        private readonly IBaseService<T, TT> _service;
        public IBaseService<T, TT> Service { get; set; }

        public BaseController(IBaseService<T, TT> service)
        {
            Service = service;
        }
    }
}
