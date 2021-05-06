using API.Data;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            foreach (PostTag tag in post.Tags) 
            {
                var tagExist = await _dataContext.Tags.SingleOrDefaultAsync(x => x.Name == tag.TagName);
                if (tagExist == null) 
                {
                    await _dataContext.Tags.AddAsync(new Tag
                    {
                        Name = tag.TagName,
                        CreatedOn = DateTime.UtcNow,
                        CreatorId = post.UserId
                    });
                    await _dataContext.PostTags.AddAsync(tag);
                }
            }
            await _dataContext.Posts.AddAsync(post);

            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var posts = await _dataContext.Posts.ToListAsync();

            foreach (Post post in posts) 
            {
                var postTags = await GetPostTagsAsync(post.Id);
                post.Tags = postTags;
            }
            return posts;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> UserOwnsPstAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
                return false;

            if (post.UserId != userId)
                return false;

            return true;
        }

        public async Task<List<Tag>> GetAllTagAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName);
        }

        public async Task<bool> CreateTagAsync(Tag newTag)
        {
            await _dataContext.Tags.AddAsync(newTag);

            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await GetTagByNameAsync(tagName);

            if (tag == null)
                return false;

            _dataContext.Tags.Remove(tag);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<PostTag>> GetPostTagsAsync(Guid postId) 
        {
            List<PostTag> postTags = new List<PostTag>();
            await _dataContext.PostTags.ForEachAsync(x =>
            {
                if (x.PostId == postId) 
                {
                    postTags.Add(x);
                }
            });
            return postTags;
        }
    }
}
