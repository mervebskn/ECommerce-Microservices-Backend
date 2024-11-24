using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Tests.Integration
{
    namespace ProductServiceTests
    {
        public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>> 
        {
            private readonly HttpClient _client;

            public ProductControllerTests(WebApplicationFactory<Program> factory)
            {
                _client = factory.CreateClient();  //send request to httpclient with api
            }

            [Fact]
            public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
            {
                int productId = 1;

                var response = await _client.GetAsync($"/api/products/{productId}");

                response.EnsureSuccessStatusCode();  // 200-299 code must be success
                var product = await response.Content.ReadAsStringAsync();
                Assert.Contains("Product Name", product);  //
            }

            [Fact]
            public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
            {
                int productId = 999; //there is not product with this id

                var response = await _client.GetAsync($"/api/products/{productId}");

                Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }

}
