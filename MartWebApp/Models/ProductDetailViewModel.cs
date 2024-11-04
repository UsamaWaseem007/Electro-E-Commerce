namespace MartWebApp.Models
{
	public class ProductDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public List<string> Images { get; set; } // List of image URLs

		public List<CartItem> CartItems { get; set; } // Cart details
	 // New property for other products
		public List<RelatedProductViewModel> Otherproduct { get; set; } // Assuming Product is your product entity
	}
}
