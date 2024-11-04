using MartWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MartWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
	{
        private readonly MartWebAppDbContext _DbContext;

        public AdminProductController(MartWebAppDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }


        public IActionResult Index()
		{

            var products = _DbContext.Products.Include(p => p.ProductImages).ToList();
         

            return View(products);
		}
		public IActionResult AddProduct()
		{

          
			var cat = _DbContext.Categories.ToList();

			ViewBag.Cat = cat;
            return View();
		}

        public IActionResult Pendingorders()
        {
            var pendingorder = _DbContext.BillingDetails.ToList();

            
            return View(pendingorder);
        }
        public IActionResult Deliverdorders()
        {
            var deliverdorder = _DbContext.BillingDetails.ToList();
            return View(deliverdorder);
        }

        public async Task<IActionResult> Orders()
        {
            var billingDetails = await _DbContext.BillingDetails.Include(b => b.Product) // Change to your navigation property
          .ToListAsync();

            //var billingdetails = _DbContext.BillingDetails.Include(b => b.ProductName)//Assuming you have a navigation property for Product
            //    .Select(b => new BillingDetails
            //    {
            //        Id = b.Id,
            //        FirstName = b.FirstName,
            //        LastName = b.LastName,
            //        Email = b.Email,
            //        Address = b.Address,
            //        City = b.City,
            //        Country = b.Country,
            //        ZipCode = b.ZipCode,
            //        Telephone = b.Telephone,
            //        productid = b.productid,
            //        ProductName = b.Product.Name  // Adjust according to your Product model
            //    });

            return View(billingDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product newproduct, IFormFile[]  productImages)
        {
            newproduct.CreatedDate = DateTime.Now; // Set the created date
              // Save the product details
            _DbContext.Products.Add(newproduct);
            await _DbContext.SaveChangesAsync();

            if (productImages != null && productImages.Length > 0)
            {
                foreach (var image in productImages)
                {
                    // Check if the image is valid
                    if (image.Length > 0)
                    {
                        // Save the image to server
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine("wwwroot/img/Product", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Create ProductImage object and add it to the database
                        var productImage = new ProductImage
                        {
                            ImageUrl = "/img/Product/" + fileName,
                            ProductId = newproduct.Id // Set the ProductId here
                        };

                        // Save to the database
                        _DbContext.ProductImages.Add(productImage);
                        _DbContext.SaveChanges();
                    }
                }
            }

            //Generate Alert While Product Add Seccessfully
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task <IActionResult> Edit( int id)
		{
            var cat = _DbContext.Categories.ToList();

            ViewBag.Cat = cat;
            var product = await _DbContext.Products
         .Include(p => p.ProductImages) // Include ProductImages if needed
         .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) 
            {
                return NotFound();
            }
            return View(product);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product updatedProduct, IFormFile[] productImages)
        {
            if (ModelState.IsValid)
            {
                // Database se existing product ko dhoondhein
                var existingProduct = await _DbContext.Products
                    .Include(p => p.ProductImages) // Include images for deletion
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Save the updated product data
                _DbContext.Entry(existingProduct).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                // Check if there are new images to process
                if (productImages != null && productImages.Length > 0)
                {
                    // Delete existing images
                    foreach (var existingImage in existingProduct.ProductImages)
                    {
                        // Optionally, delete the physical file
                        var oldFilePath = Path.Combine("wwwroot/img/product", existingImage.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        // Remove from database
                        _DbContext.ProductImages.Remove(existingImage);
                    }

                    // Save changes after deleting existing images
                    await _DbContext.SaveChangesAsync();

                    // Save new images
                    foreach (var image in productImages)
                    {
                        var filePath = Path.Combine("wwwroot/img/product", image.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream); // Save the image file
                        }
                        existingProduct.ProductImages.Add(new ProductImage { ImageUrl = "/img/product/" + image.FileName });
                    }
                    await _DbContext.SaveChangesAsync();
                }

              
                TempData["updateMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            // Agar model state valid nahi hai, to form wapas dikhaen
            return View(updatedProduct);
        }


        [HttpGet]
		public IActionResult Delete(int ?id)
		{
            if( id > 0)
            {
                var targatedrecord = _DbContext.Products.Find(id);
                if(targatedrecord != null)
                {
                    _DbContext.Remove(targatedrecord);
                    _DbContext.SaveChanges();
                    TempData["SucessfullyDelete"] = "Delete Successfully!!";
                    RedirectToAction("index");
                }
                else
                {
                    TempData["404"] = "Product Not Found !!";
                    RedirectToAction("index");
                }
                
            }
            else
            {
                TempData["idissue"] = "Invalid Product Id!!";
                RedirectToAction("index");
            }
           
            

			return RedirectToAction("index");
        }
	}
}
