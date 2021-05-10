using Api.Contracts.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class HealthCheckExtension
    {
        public static void UseDbHeathCheck(this IApplicationBuilder applicationBuilder) 
        {
            applicationBuilder.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }
    }
}
