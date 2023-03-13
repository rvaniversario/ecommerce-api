using EcommerceApi.Context;

namespace EcommerceApi.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        protected readonly AppDbContext _context;

        public BasicAuthMiddleware(RequestDelegate next, AppDbContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (StringCompare(context, "/api/v1/users"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("x-user-id"))
            {
                context.Response.StatusCode = 401;
                return;
            }

            string userId = context.Request.Headers["x-user-id"];
            Guid parsedUserId = Guid.Parse(userId);

            if(!IsValidUserId(parsedUserId))
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }

        private static bool StringCompare(HttpContext context, string route)
        {
            var stringComparison = context.Request.Path.Equals(route, StringComparison.OrdinalIgnoreCase);
            return stringComparison;
        }

        private bool IsValidUserId(Guid userId)
        {
            var user = _context.Users!.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
