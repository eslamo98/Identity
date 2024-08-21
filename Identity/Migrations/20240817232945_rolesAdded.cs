using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class rolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "54c0a014-696a-47e9-87a3-597b53a40f11", Guid.NewGuid().ToString(), "Guest", "GUEST" },
                    { "a1d67227-f31a-4d28-95ad-ac989b5f15f8", Guid.NewGuid().ToString(), "User", "USER" },
                    { "b1cb1e84-b9d3-4717-9a00-eae328e5d115", Guid.NewGuid().ToString(), "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "54c0a014-696a-47e9-87a3-597b53a40f11");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a1d67227-f31a-4d28-95ad-ac989b5f15f8");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b1cb1e84-b9d3-4717-9a00-eae328e5d115");
        }
    }
}
