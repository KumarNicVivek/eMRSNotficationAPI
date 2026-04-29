using SERVICEAPP.ServiceLayer;

namespace emrsCandidateService.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private RequestDelegate _next;

        public VisitorTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IVisitorService visitorService)
        {
            //var path = context.Request.Path.Value?.ToLower() ?? "";

            //// Skip unwanted routes
            //if (path.Contains("/api/auth") || path.Contains("/api/admin"))
            //{
            //    await _next(context);
            //    return;
            //}

            var path = context.Request.Path.ToString().ToLower();

            if (path.Contains("/api/auth") || path.Contains("/api/admin"))
            {
                await _next(context);
                return;
            }


            int toatlVisitors = await visitorService.TrackVisitorAsync(context);

            context.Items["TotalVisitors"] = toatlVisitors;

            // Add as response header (global access)
            context.Response.Headers["X-Total-Visitors"] = toatlVisitors.ToString();

            await _next(context);

        }
    }
}
