namespace MartWebApp.Models
{
	public class BillingDetails
	{
		public int Id { get; set; } // Primary key for the table
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
		public string Telephone { get; set; }
		public int productid { get; set; }
        public int status { get; set; }
        public string ProductName { get; set; }

        // Ensure this is correctly set up
        public virtual Product Product { get; set; }  // Assuming Product is the related entity


    }
}
