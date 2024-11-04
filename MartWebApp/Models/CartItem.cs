namespace MartWebApp.Models
{
	public class CartItem
	{
		public int CartItemId { get; set; }
		public int CartId { get; set; } // Foreign key to Cart
		public int ProductId { get; set; } // Foreign key to Product
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public virtual Cart Cart { get; set; } // Navigation property
		public virtual Product Product { get; set; } // Navigation property
	}

}
