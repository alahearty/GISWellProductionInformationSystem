using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Orbit.API {
    public class ExceptionHandler {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception) {
            // Log exception here
            string result = JsonConvert.SerializeObject(ResponseBody.Error(exception.Message), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
