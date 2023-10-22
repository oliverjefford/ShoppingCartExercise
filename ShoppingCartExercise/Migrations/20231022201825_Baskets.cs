using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartExercise.Migrations
{
    /// <inheritdoc />
    public partial class Baskets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProductBarcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => new { x.Id, x.ProductBarcode });
                    table.ForeignKey(
                        name: "FK_Baskets_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Baskets_Products_ProductBarcode",
                        column: x => x.ProductBarcode,
                        principalTable: "Products",
                        principalColumn: "Barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_OfferId",
                table: "Baskets",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ProductBarcode",
                table: "Baskets",
                column: "ProductBarcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Baskets");
        }
    }
}
