using API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SwagerExamples.Requests
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                TagName = "new tag"
            };
        }
    }
}
