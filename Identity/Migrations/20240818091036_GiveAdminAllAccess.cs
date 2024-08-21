using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class GiveAdminAllAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[UserRoles] (UserId, RoleId) SELECT 'f1935da1-eef6-423a-ac3b-3375f20d34af', Id \r\nFROM [user_managementWith_identity].[Security].[Roles];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Security].[UserRoles] where  UserId = 'f1935da1-eef6-423a-ac3b-3375f20d34af'");
        }
    }
}
