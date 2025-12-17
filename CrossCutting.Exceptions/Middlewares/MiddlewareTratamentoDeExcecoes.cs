using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CrossCutting.Exceptions.Middlewares
{
    public class MiddlewareTratamentoDeExcecoes
    {
        private readonly RequestDelegate _next;

        public MiddlewareTratamentoDeExcecoes(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteResponseAsync(context, ex.Errors);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteResponseAsync(context, ex.Message);
            }
            catch (BusinessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                await WriteResponseAsync(context, ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await WriteResponseAsync(
                    context,
                    ex.Message);
            }
        }

        private static async Task WriteResponseAsync(
            HttpContext context,
            object error)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                errors = error
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
