using API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(GetAllPostsFillter filter=null, PaginationFilter paginationFilter = null);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);

        Task<bool> UserOwnsPostAsync(Guid postId, string userId);

        Task<Tag> GetTagByNameAsync(string tagName);
        Task<bool> CreateTagAsync(Tag newTag);
        Task<bool> DeleteTagAsync(string tagName);

        Task<List<Tag>> GetAllTagAsync();

        Task<List<PostTag>> GetPostTagsAsync(Guid postId);
    }
}
