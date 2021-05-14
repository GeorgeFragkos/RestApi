using Api.Contracts.V1.Responses;
using API.Contracts.V1;
using API.Contracts.V1.Requests;
using API.Contracts.V1.Responses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestApiIntegration
{
    public class IntegrationTest
    {
        protected readonly HttpClient Client;

        public IntegrationTest(HttpClient client)
        {
            Client = client;
        }

        public async Task AuthenticateAsync()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            await Client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {

                Email = "test@test.com",
                Password = "Test12345@"
            });

            var loginResponse = await Client.PostAsJsonAsync(ApiRoutes.Identity.Login, new UserLoginRequest
            {

                Email = "test@test.com",
                Password = "Test12345@"
            });

            var response = await loginResponse.Content.ReadAsAsync<AuthSuccessResponse>();
            return response.Token;
        }

        public  async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response =await Client.PostAsJsonAsync(ApiRoutes.Posts.Create, request);

            var postResponse = await response.Content.ReadAsAsync<Response<PostResponse>>();

            return postResponse.Data;
        }
    }
}
