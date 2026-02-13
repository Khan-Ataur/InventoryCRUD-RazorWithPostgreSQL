using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductCRUDrazorWithPostgresql.Models;
using ProductCRUDrazorWithPostgresql.Repository;

namespace ProductCRUDrazorWithPostgresql.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IProductRepository productRepository, ILogger<EditModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [BindProperty]
        public ProductUpdateDto Product { get; set; } = new ProductUpdateDto();

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

                Product = new ProductUpdateDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product {ProductId}", id);
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _productRepository.UpdateProductAsync(Product);
                
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Product not found.");
                    return Page();
                }

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", Product.Id);
                ModelState.AddModelError(string.Empty, "Error updating product. Please try again.");
                return Page();
            }
        }
    }
}
