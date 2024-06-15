using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Product.API.Controllers.Base;
using Product.Domain.Interfaces.Services;
using Product.Domain.Secutiry;

namespace Product.API.Filter
{
    public class TokenHandlerAttribute : TypeFilterAttribute
    {
        public TokenHandlerAttribute() : base(typeof(AuthrorizeFilter)) { }
    }

    public class AuthrorizeFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var claim = context.HttpContext.User.Claims.FirstOrDefault(f => f.Type.Equals("context"));
            if (claim is null || string.IsNullOrEmpty(claim.Value) || context.Controller is not BaseController)
                return;


            var controller = (BaseController)context.Controller;
            var service = controller.GetType().GetProperty("Service")?.GetValue(controller);
            
            if (service is null)
                return;

            var jwtContext = JsonConvert.DeserializeObject<JwtContextVO>(claim.Value);
            
            ((IBaseServiceUserHandler)service).SetJwtContext(jwtContext);
            ((IBaseServiceUserHandler)service).Repository?.SetJwtContext(jwtContext);
        }
    }
}
