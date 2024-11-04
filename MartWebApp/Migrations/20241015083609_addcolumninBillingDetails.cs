using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MartWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addcolumninBillingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<int>(
		name: "status",
		table: "BillingDetails",
		nullable: false,
		defaultValue: 0); // Aap default value change kar sakte hain agar zaroorat ho
            migrationBuilder.AddColumn<string>(
    name: "ProductName",
    table: "BillingDetails",
    nullable: false); // Aap default value change kar sakte hain agar zaroorat ho
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
