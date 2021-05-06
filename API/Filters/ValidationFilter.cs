using API.Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState.
                    Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
                var errorResponse = new ErrorResponse();

                foreach (var error in errorsInModelState)
                {
                    foreach (var subEroor in error.Value) 
                    {
                        var errorModel = new ErrorModel
                        {
                            FiledName = error.Key,
                            Message = subEroor
                        };
                        errorResponse.Errors.Add(errorModel);
                    }
                    
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
