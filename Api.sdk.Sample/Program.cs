using Api.Sdk;
using API.Contracts.V1.Requests;
using Refit;
using System.Threading.Tasks;

namespace Api.sdk.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var api = RestService.For<IApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "george@gmail.com",
                Password = "Geo1234@"
            });
            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "george@gmail.com",
                Password = "Geo1234@"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await api.GetAllAsync();
            var createdPost = await api.CreateAsync(new CreatePostRequest
            {
                Name = "Created by SDK",
                Tags = new[] { "sdk" }
            });
            var retrievedPost = await api.GetAsync(createdPost.Content.Id);
            var updatedPost = await api.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest
            {
                Name = "Updated by SDK"
            });

            var deletedPost = await api.DeleteAsync(createdPost.Content.Id);
        }
    }
}
