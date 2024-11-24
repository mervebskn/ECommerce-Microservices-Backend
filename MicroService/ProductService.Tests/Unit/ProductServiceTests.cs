using Common.Models;
using Moq;
using ProductService.Abstractions;
using ProductService.Services;
using FluentAssertions;
using Common.Exceptions;

namespace ProductService.Tests.Unit
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProdService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _productService = new ProdService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProductDto_WhenProductExists()
        {
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.50m,
                Stock = 10
            };

            _mockRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(product);

            var result = await _productService.GetProductById(productId);

            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
            result.Name.Should().Be(product.Name);
            result.Description.Should().Be(product.Description);
            result.Price.Should().Be(product.Price);
            result.Stock.Should().Be(product.Stock);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var productId = 1;

            _mockRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync((Product)null);

            var result = await _productService.GetProductById(productId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetProductById_ShouldThrowNotFoundException_WhenProductDoesNotExistAndThrowingEnabled()
        {
            var productId = 1;

            _mockRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync((Product)null);

            var act = async () => await _productService.GetProductById(productId);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Product not found");
        }
    }

}