namespace MartWebApp.Models
{
	public class Cart
	{
		public int CartId { get; set; }
		public string UserId { get; set; } // Foreign key for the user
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
	}
}
