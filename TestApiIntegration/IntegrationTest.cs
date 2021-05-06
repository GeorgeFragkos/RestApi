using API.Contracts.V1;
using API.Contracts.V1.Requests;
using API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            var response = await Client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {

                Email = "test@test.com",
                Password = "Test12345@"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        public  async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response =await Client.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
            return await response.Content.ReadAsAsync<PostResponse>();
        }
    }
}
