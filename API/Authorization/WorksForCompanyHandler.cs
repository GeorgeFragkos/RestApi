using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Authorization
{
    public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirment>
    {

        /// <summary>
        /// if the email ends whith something i require authorize else reject me
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        /// 

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirment requirement)
        {
            var userEmailAddress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if (userEmailAddress.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
