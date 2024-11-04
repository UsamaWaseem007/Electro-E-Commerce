using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MartWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addcolumninBillingDetails2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "BillingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

           

            migrationBuilder.CreateIndex(
                name: "IX_BillingDetails_ProductId",
                table: "BillingDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingDetails_Products_ProductId",
                table: "BillingDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingDetails_Products_ProductId",
                table: "BillingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillingDetails_ProductId",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "status",
                table: "BillingDetails");
        }
    }
}
