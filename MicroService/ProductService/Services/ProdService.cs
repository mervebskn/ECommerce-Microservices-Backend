using Common.DTOs;
using Common.Models;
using ProductService.Abstractions;

namespace ProductService.Services
{
    public class ProdService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProdService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            if (products == null)
            {
                return new List<ProductDto>();
            }
            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            }).ToList();
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return null; // veya throw new NotFoundException("Product not found");
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productRepository.CreateProduct(product);
            return productDto; // Convert to DTO as necessary
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetProductById(id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Stock = productDto.Stock;
                product.UpdatedAt = DateTime.UtcNow;

                await _productRepository.UpdateProduct(product);
            }
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }
    }
}
