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
    public class PostsContoller : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostsContoller(IPostService postservice, IMapper mapper)
        {
            _postService = postservice;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();

            return Ok(_mapper.Map<List<PostResponse>>(posts));
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            var tags = await _postService.GetPostTagsAsync(postId);

            post.Tags = tags;

            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newpostId = Guid.NewGuid();
            var post = new Post { 
                Id = newpostId,
                Name = postRequest.Name, 
                UserId =HttpContext.GetUserId(),Tags=postRequest.Tags.
                Select(x=> new PostTag {PostId = newpostId,TagName = x }).ToList() 
            };

            
            await _postService.CreatePostAsync(post);
            
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUril = baseUrl + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = _mapper.Map<PostResponse>(post);
            return Created(locationUril, response);
        }
        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {

            var userOwnsPost = await _postService.UserOwnsPstAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You do not own this post" });


            var post = await _postService.GetPostByIdAsync(postId);
            var tags = await _postService.GetPostTagsAsync(post.Id);
            post.Name = request.Name;
            post.Tags = tags;
            
            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(_mapper.Map<PostResponse>(post));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {

            var userOwnsPost = await _postService.UserOwnsPstAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You do not own this post" });

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
