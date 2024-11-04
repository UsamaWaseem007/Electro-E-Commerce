namespace MartWebApp.Models
{
	public class RelatedProductViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string FirstImageUrl { get; set; } // To hold the first image URL
	}
}
