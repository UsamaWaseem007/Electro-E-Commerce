using MartWebApp.Migrations;
using MartWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MartWebApp.Controllers
{
	
    public class CheckoutController : Controller
	{

		private readonly MartWebAppDbContext _DbContext;

		public CheckoutController(MartWebAppDbContext dbcontext)
		{
			_DbContext = dbcontext;
		}


		public IActionResult OrderConfirm()
		{
			return View();
		}


		[Authorize]
        public async Task<IActionResult> CheckOut( int? id)
		{
			

			var product = await _DbContext.Products.FindAsync(id);

			var viewModel = new ProductBillingDetailViewModel
			{
				product = product,
				billingDetails = new BillingDetails() // Create an empty BillingDetails object or populate it if needed
			};
			return View(viewModel);
		}
        [Authorize]
        [HttpPost]
		public async Task<IActionResult> Placeorder(int productId, ProductBillingDetailViewModel neworder)
		
		
		{
			var selectedproduct = await _DbContext.Products.FindAsync(productId);
			if(neworder != null)
			{
				var billingDetails = new BillingDetails
				{

					FirstName = neworder.billingDetails.FirstName,
					LastName = neworder.billingDetails.LastName,
					Email = neworder.billingDetails.Email,
					Address = neworder.billingDetails.Address,
					City = neworder.billingDetails.City,
					Country = neworder.billingDetails.Country,
					ZipCode = neworder.billingDetails.ZipCode,
					Telephone = neworder.billingDetails.Telephone,
					// for prodct identifier
					productid = productId,

				};
				await _DbContext.BillingDetails.AddAsync(billingDetails);
				await _DbContext.SaveChangesAsync();
				TempData["SuccessMessage"] = "Your Order has been Successfully Plaeced !!";
				return RedirectToAction("index","home");

				{


				}


			}

			return RedirectToAction("OrderConfirm");
		}
	}
}
