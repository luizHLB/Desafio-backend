using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Product.Domain.Exceptions;
using Product.Domain.Secutiry;
using System.Diagnostics;
using System.Text;

namespace Product.API.Middlewares
{
    public static class GlobalExceptionHandlerMiddleware
    {
        public static string BodyAsText { get; private set; }

        public static void UseCaptureRequestBodyMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                BodyAsText = await RequestBodyAsync(context.Request);

                await next.Invoke();
            });
        }

        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app, ILogger logger, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var configuration = (IConfiguration)context.RequestServices.GetService(typeof(IConfiguration));

                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature == null)
                        return;

                    var exception = exceptionHandlerFeature.Error;

                    ProblemDetails problem;

                    switch (exception)
                    {
                        case LoginException _:
                            problem = new ProblemDetails
                            {
                                Type = $"{context.Request.Scheme}://{context.Request.Host}",
                                Instance = $"{context.Request.Path}{context.Request.QueryString}",
                                Title = "Login failed",
                                Status = StatusCodes.Status401Unauthorized,
                                Detail = exception.Message,
                            };
                            break;
                        case RecordNotFoundException _:
                            problem = new ProblemDetails
                            {
                                Type = $"{context.Request.Scheme}://{context.Request.Host}",
                                Instance = $"{context.Request.Path}{context.Request.QueryString}",
                                Title = "Record not found",
                                Status = StatusCodes.Status404NotFound,
                                Detail = exception.Message,
                            };
                            break;
                        case EntityConstraintException _:
                            problem = new ProblemDetails
                            {
                                Type = $"{context.Request.Scheme}://{context.Request.Host}",
                                Instance = $"{context.Request.Path}{context.Request.QueryString}",
                                Title = "Invalid Data",
                                Status = StatusCodes.Status400BadRequest,
                                Detail = exception.Message,
                            };
                            break;
                        case SecurityTokenSignatureKeyNotFoundException _:
                            problem = new ProblemDetails
                            {
                                Type = $"{context.Request.Scheme}://{context.Request.Host}",
                                Instance = $"{context.Request.Path}{context.Request.QueryString}",
                                Title = "Invalid Token",
                                Status = StatusCodes.Status401Unauthorized,
                                Detail = "Invalid Token",
                            };
                            break;
                        default:
                            problem = new ProblemDetails
                            {
                                Type = $"{context.Request.Scheme}://{context.Request.Host}",
                                Instance = $"{context.Request.Path}{context.Request.QueryString}",
                                Title = !env.IsProduction() ? exception.Message : "An unexpected error occurred!",
                                Status = StatusCodes.Status500InternalServerError,
                                Detail = !env.IsProduction() ? JsonConvert.SerializeObject(exception): "Contact your system administrator",
                            };
                            break;
                    }

                    // logger
                    logger.LogError($"Unexpected error: {exception}");

                    var command = new CreateLoggerErrorCommand
                    {
                        TraceId = Activity.Current?.Id ?? context.TraceIdentifier,
                        IpAddress = context.Connection.RemoteIpAddress.ToString(),
                        RequestBody = BodyAsText,
                        CreatedBy = "GlobalExceptionHandler",
                        CreatedAt = DateTime.Now,
                        Data = problem
                    };

                    //TODO: insert error in a database

                    if (env.IsProduction())
                        problem.Extensions.Add("traceId", command.TraceId);

                    context.Response.StatusCode = problem.Status.Value;
                    context.Response.ContentType = "application/problem+json";

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(problem, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }), Encoding.UTF8);
                });
            });
        }

        private static async Task<string> RequestBodyAsync(HttpRequest request)
        {
            var bodyAsText = string.Empty;

            request.EnableBuffering();

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                bodyAsText = await reader.ReadToEndAsync();

                request.Body.Position = 0;
            }

            return bodyAsText;
        }

        internal class CreateLoggerErrorCommand
        {
            public string Language { get; set; }

            [JsonIgnore] public JwtContextVO JwtContext { get; set; }

            public string TraceId { get; set; }

            public string IpAddress { get; set; }

            public string RequestBody { get; set; }

            public string CreatedBy { get; set; }

            public DateTime CreatedAt { get; set; }

            public object Data { get; set; }
        }
    }
}
