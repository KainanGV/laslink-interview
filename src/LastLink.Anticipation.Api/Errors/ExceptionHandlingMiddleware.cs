using System.Net;
using System.Text.Json;
using LastLink.Anticipation.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.API.Errors
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (AppValidationException vex)
            {
                await WriteProblem(ctx, HttpStatusCode.BadRequest, "Validation error", vex.Message, extras: new()
                {
                    ["errors"] = vex.Errors
                });
            }
            catch (NotFoundException nf)
            {
                await WriteProblem(ctx, HttpStatusCode.NotFound, "Resource not found", nf.Message);
            }
            catch (ConflictException cf)
            {
                await WriteProblem(ctx, HttpStatusCode.Conflict, "Conflict", cf.Message);
            }
            catch (Exception ex)
            {
                await WriteProblem(ctx, HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
        }

        private static async Task WriteProblem(
            HttpContext ctx,
            HttpStatusCode status,
            string title,
            string detail,
            Dictionary<string, object>? extras = null)
        {
            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = detail
            };

            if (extras != null)
            {
                foreach (var kv in extras)
                    problem.Extensions[kv.Key] = kv.Value;
            }

            ctx.Response.StatusCode = problem.Status!.Value;
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
        }
    }

    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
