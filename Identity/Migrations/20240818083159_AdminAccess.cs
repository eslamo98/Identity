using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class AdminAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[Users] ([Id],[FirstName],[LastName],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount])VALUES ('f1935da1-eef6-423a-ac3b-3375f20d34af','Admin','Admin','Admin','ADMIN','Admin@gmail.com','ADMIN',1,'AQAAAAIAAYagAAAAEEN36vnfWZLdLazYKytH4Pn5SFStWc5+grpRqCx2ad2U/qICrqnAN3BOVaSSAxqp3g==','T5NGWNQTCHI6WJID3S7LZZXPFQ2CCOQO','30448a08-5c08-4469-9cb2-ce6082e3990f','0106 928 2307',0,0,NULL,1,0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Security].[Users] where id='f1935da1-eef6-423a-ac3b-3375f20d34af'");
        }
    }
}
