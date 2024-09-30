using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Presentation.Middleware
{
    // RedirectToAuthMiddleware.cs
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class RedirectToAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectToAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                // Redirect to the authorization app
                context.Response.Redirect("http://localhost:5000/Login");
                return;
            }

            await _next(context);
        }
    }

}
