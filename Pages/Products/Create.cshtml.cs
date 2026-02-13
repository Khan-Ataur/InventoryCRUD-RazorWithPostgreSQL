using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductCRUDrazorWithPostgresql.Models;
using ProductCRUDrazorWithPostgresql.Repository;

namespace ProductCRUDrazorWithPostgresql.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IProductRepository productRepository, ILogger<CreateModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [BindProperty]
        public ProductCreateDto Product { get; set; } = new ProductCreateDto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _productRepository.CreateProductAsync(Product);
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, "Error creating product. Please try again.");
                return Page();
            }
        }
    }
}
