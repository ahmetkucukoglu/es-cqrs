namespace ESSample.API.Infrastructure
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                {
                    var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();
                    
                    var errorDetail = new
                    {
                        context.Response.StatusCode,
                        exception.Message,
                        requestIdFeature?.TraceIdentifier
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetail));
                }
            }
        }
    }
}
