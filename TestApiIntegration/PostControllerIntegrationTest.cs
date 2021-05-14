using Api.Contracts.V1.Responses;
using API;
using API.Contracts.V1;
using API.Contracts.V1.Requests;
using API.Contracts.V1.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestApiIntegration
{
    public class PostControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient Client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly IntegrationTest _integrationTest;

        public PostControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _integrationTest = new IntegrationTest(Client);
        }

        [Fact]
        public async Task GetAll_WhithoutAnyPost_ReternsEmptyResponse()
        {
            //Arrange
            await _integrationTest.AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Posts.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var desirializedResponse = await response.Content.ReadAsAsync<PagedResponse<PostResponse>>();
            desirializedResponse.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInDataBase() 
        {
            //Arrange
            await _integrationTest.AuthenticateAsync();

            var tags = new List<string> { "tag"};
            var createdPost =await _integrationTest.CreatePostAsync(new CreatePostRequest 
            {
                Name = "Test Post",
                Tags = tags
            });

            //Act
            var response = await Client.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}",createdPost.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var serializedResponse = await response.Content.ReadAsAsync<Response<PostResponse>>();

            serializedResponse.Data.Id.Should().Be(createdPost.Id);
        }

        [Fact]
        public async Task Dellete_ReturnNoContent_WhenPostDelletedSuccessful() 
        {
            //Arrange
            await _integrationTest.AuthenticateAsync();
            var tags = new List<string> { "tag" };
            var createPost = await _integrationTest.CreatePostAsync(new CreatePostRequest
            {
                Name = "Post to Delete",
                Tags = tags
            });
            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Posts.Delete.Replace("{postId}", createPost.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }  
    }
}