using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductCRUDrazorWithPostgresql.Models;
using ProductCRUDrazorWithPostgresql.Repository;

namespace ProductCRUDrazorWithPostgresql.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IProductRepository productRepository, ILogger<DetailsModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public Product Product { get; set; } = new Product();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id.Value);

                if (product == null)
                {
                    return NotFound();
                }

                Product = product;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product {ProductId}", id);
                return RedirectToPage("./Index");
            }
        }
    }
}
