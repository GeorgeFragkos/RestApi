using API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace API.SwagerExamples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "new tag"
            };
        }
    }
}
