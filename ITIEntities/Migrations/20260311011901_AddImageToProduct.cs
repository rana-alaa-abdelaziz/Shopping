using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITIEntities.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b34c217a-fea3-426e-bb0e-0c78abf04311");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd39b9b8-2776-4358-9542-db3a3157ac51");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a69e2d29-1678-4903-9dd0-b459e09c2403", "454a0a78-5687-462e-928c-aabf436b2750", "Admin", "ADMIN" },
                    { "a88c4d09-cea8-4771-9975-6aaeff3cdcbe", "24a72fdc-95d7-4009-8a32-58a20b12f1ad", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a69e2d29-1678-4903-9dd0-b459e09c2403");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a88c4d09-cea8-4771-9975-6aaeff3cdcbe");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b34c217a-fea3-426e-bb0e-0c78abf04311", "8e5a7ba5-61b3-4595-baa8-b6af3a79a61b", "Admin", "ADMIN" },
                    { "dd39b9b8-2776-4358-9542-db3a3157ac51", "437ce619-ce43-4e6a-a6e7-1557dc147649", "User", "USER" }
                });
        }
    }
}
