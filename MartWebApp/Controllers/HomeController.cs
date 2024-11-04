using MartWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MartWebApp.Controllers
{
    public class HomeController : Controller
    {
       


        private readonly MartWebAppDbContext _DbContext;

		public HomeController(MartWebAppDbContext dbcontext)
		{
			_DbContext = dbcontext;
		}


        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
         


            var productdata = _DbContext.Products.ToList();
            var products = _DbContext.Products.Select(product => new ProductViewModel
            {
                Id=product.Id,
                Name = product.Name,
               Description =  product.Description,
                 Price =product.Price,
                StockQuantity = product.StockQuantity,
                //get the first image for home display 
               ImageUrl = product.ProductImages.Select(img => img.ImageUrl).FirstOrDefault()

            }).ToList();

            return View(products);
        }
        [HttpGet]
		[Route("Product/Details/{id}")]
		public async Task<IActionResult> ProductDetail(int id)
		{
			// Specific product ko retrieve karein
			var product = await _DbContext.Products
				.Where(p => p.Id == id)
				.Select(p => new ProductDetailViewModel
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					StockQuantity = p.StockQuantity,
					Images = p.ProductImages.Select(img => img.ImageUrl).ToList()
				})
				.FirstOrDefaultAsync();

			if (product == null)
			{
				return NotFound();
			}

			// Retrieve other products (for example, products in the same category or different products)
			var otherProducts = await _DbContext.Products
				.Where(p => p.Id != id) // Exclude the current product
				.Select(p => new RelatedProductViewModel
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					FirstImageUrl = p.ProductImages != null && p.ProductImages.Any()
						? p.ProductImages.FirstOrDefault().ImageUrl // Retrieve the first image URL
						: "placeholder.jpg" // Default image if no images are available
				})
				.ToListAsync();

			// Assign other products to the view model
			product.Otherproduct = otherProducts;




			return View(product);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
