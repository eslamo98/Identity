using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class PicAllowNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                schema: "Security",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22c99a38-f77e-493e-9232-6d7054ea2cce", null, "Guest", "GUEST" },
                    { "3533f973-1bc8-4fb0-b553-db3a356aa895", null, "User", "USER" },
                    { "cd46ff21-6813-4da8-a123-b80d67c08602", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "22c99a38-f77e-493e-9232-6d7054ea2cce");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3533f973-1bc8-4fb0-b553-db3a356aa895");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cd46ff21-6813-4da8-a123-b80d67c08602");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                schema: "Security",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "54c0a014-696a-47e9-87a3-597b53a40f11", null, "Guest", "GUEST" },
                    { "a1d67227-f31a-4d28-95ad-ac989b5f15f8", null, "User", "USER" },
                    { "b1cb1e84-b9d3-4717-9a00-eae328e5d115", null, "Admin", "ADMIN" }
                });
        }
    }
}
