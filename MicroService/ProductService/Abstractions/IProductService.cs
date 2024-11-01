using Common.DTOs;

namespace ProductService.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateProduct(ProductDto productDto);
        Task UpdateProduct(int id, ProductDto productDto);
        Task DeleteProduct(int id);
    }
}
