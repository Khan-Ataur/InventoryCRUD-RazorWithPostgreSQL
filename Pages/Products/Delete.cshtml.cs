using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductCRUDrazorWithPostgresql.Models;
using ProductCRUDrazorWithPostgresql.Repository;

namespace ProductCRUDrazorWithPostgresql.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IProductRepository productRepository, ILogger<DeleteModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var result = await _productRepository.DeleteProductAsync(id.Value);

                if (!result)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Product deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                ModelState.AddModelError(string.Empty, "Error deleting product. Please try again.");
                return Page();
            }
        }
    }
}
