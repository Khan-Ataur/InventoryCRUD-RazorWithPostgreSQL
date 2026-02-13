using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductCRUDrazorWithPostgresql.Models;
using ProductCRUDrazorWithPostgresql.Repository;

namespace ProductCRUDrazorWithPostgresql.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductRepository productRepository, ILogger<IndexModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Products = await _productRepository.GetAllProductsAsync();
                
                if (TempData["SuccessMessage"] != null)
                {
                    SuccessMessage = TempData["SuccessMessage"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                ErrorMessage = "Error loading products. Please try again.";
                Products = new List<Product>();
            }
        }
    }
}
