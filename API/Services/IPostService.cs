using API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);

        Task<bool> UserOwnsPstAsync(Guid postId, string userId);

        Task<Tag> GetTagByNameAsync(string tagName);
        Task<bool> CreateTagAsync(Tag newTag);
        Task<bool> DeleteTagAsync(string tagName);

        Task<List<Tag>> GetAllTagAsync();

        Task<List<PostTag>> GetPostTagsAsync(Guid postId);
    }
}
