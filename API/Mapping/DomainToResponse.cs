using API.Contracts.V1.Responses;
using API.Domain;
using AutoMapper;
using System.Linq;

namespace API.Mapping
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(destination => destination.Tags, options => 
                options.MapFrom(src => src.Tags.Select(x => new TagResponse { Name = x.TagName})));

            CreateMap<Tag, TagResponse>();

            
        }
    }
}
