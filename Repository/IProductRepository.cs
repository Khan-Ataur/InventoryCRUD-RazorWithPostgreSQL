using ProductCRUDrazorWithPostgresql.Models;

namespace ProductCRUDrazorWithPostgresql.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(ProductCreateDto product);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> DeleteProductAsync(int id);
    }
}
