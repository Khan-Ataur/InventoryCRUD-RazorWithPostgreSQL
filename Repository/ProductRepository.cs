using Dapper;
using Npgsql;
using ProductCRUDrazorWithPostgresql.Models;
using System.Data;

namespace ProductCRUDrazorWithPostgresql.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string not found");
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM sp_get_all_products()";
            
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM sp_get_product_by_id(@p_id)";
            
            return await connection.QueryFirstOrDefaultAsync<Product>(
                sql,
                new { p_id = id }
            );
        }

        public async Task<int> CreateProductAsync(ProductCreateDto product)
        {
            using var connection = CreateConnection();
            var sql = "SELECT sp_create_product(@p_name, @p_description, @p_price, @p_stock)";
            
            var result = await connection.QuerySingleAsync<int>(
                sql,
                new { 
                    p_name = product.Name,
                    p_description = product.Description,
                    p_price = product.Price,
                    p_stock = product.Stock
                }
            );

            return result;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            using var connection = CreateConnection();
            var sql = "SELECT sp_update_product(@p_id, @p_name, @p_description, @p_price, @p_stock)";
            
            var result = await connection.ExecuteScalarAsync<int>(
                sql,
                new { 
                    p_id = product.Id,
                    p_name = product.Name,
                    p_description = product.Description,
                    p_price = product.Price,
                    p_stock = product.Stock
                }
            );

            return result > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT sp_delete_product(@p_id)";
            
            var result = await connection.ExecuteScalarAsync<int>(
                sql,
                new { p_id = id }
            );

            return result > 0;
        }
    }
}
