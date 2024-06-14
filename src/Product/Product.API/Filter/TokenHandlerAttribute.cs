using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Product.API.Controllers.Base;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Services;
using Product.Domain.Secutiry;

namespace Product.API.Filter
{
    public class TokenHandlerAttribute : TypeFilterAttribute
    {
        public TokenHandlerAttribute() : base(typeof(AuthrorizeFilter))
        {
        }
    }

    public class AuthrorizeFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var contextJson = context.HttpContext.User.Claims.FirstOrDefault(f => f.Type.Equals("context")).Value;
            var jwtContext = JsonConvert.DeserializeObject<JwtContextVO>(contextJson);

            var controller = (BaseController<Driver, DriverDTO>)context.Controller;
            var service = controller.GetType().GetProperty("Service").GetValue(controller);
            ((IBaseService<Driver, DriverDTO>)service).SetJwtContext(jwtContext);
        }
    }
}
