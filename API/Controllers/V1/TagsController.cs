using API.Contracts.V1;
using API.Contracts.V1.Requests;
using API.Contracts.V1.Responses;
using API.Domain;
using API.Extensions;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Policy = "TagViewer")]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public TagsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _postService.GetAllTagAsync();

            var tagResponses = _mapper.Map<List<TagResponse>>(tags);
            return Ok(tagResponses);
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            var tag = await _postService.GetTagByNameAsync(tagName);

            if (tag == null)
                return NotFound();

            return Ok(_mapper.Map<TagResponse>(tag));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
          
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);
            if (!created)  
                return BadRequest(new { error = "Unable to create tag" });
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);

            return Created(locationUri, _mapper.Map<TagResponse>(newTag));

        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Policy = "MustWorkForGeorge")]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }

    }
}
