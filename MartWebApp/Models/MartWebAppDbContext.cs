using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MartWebApp.Models
{
    public class MartWebAppDbContext : IdentityDbContext
    {
        public MartWebAppDbContext(DbContextOptions<MartWebAppDbContext> option) : base(option)
        {
        
        }

        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems {  get; set; }
    
      public DbSet<BillingDetails> BillingDetails { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }

		public DbSet<User>  Users { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Cart and CartItem relationship configuration
			modelBuilder.Entity<Cart>()
				.HasMany(c => c.CartItems)
				.WithOne(ci => ci.Cart)
				.HasForeignKey(ci => ci.CartId);

			// Other configurations...
			// Configure your entity relationships if needed
			// For example, if you want to configure the foreign key relationship:
			modelBuilder.Entity<BillingDetails>()
				.HasOne<Product>() // Assuming you have a Product entity
				.WithMany() // Assuming one product can have many billing details
				.HasForeignKey(b => b.productid); // Sets ProductId as the foreign key

				modelBuilder.Entity<CartItem>()
			  .Property(c => c.Price)
			  .HasColumnType("decimal(18,2)");

					modelBuilder.Entity<Product>()
					.Property(p => p.Price)
					.HasColumnType("decimal(18,2)");
		}
	}



}
    