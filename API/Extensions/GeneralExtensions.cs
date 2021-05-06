using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
