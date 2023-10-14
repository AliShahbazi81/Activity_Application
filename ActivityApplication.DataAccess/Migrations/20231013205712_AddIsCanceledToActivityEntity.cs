using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActivityApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCanceledToActivityEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("149c675d-3f89-4cb0-b360-20b7e5c46d2d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bac2dbe-31da-4957-bedb-472c8c060563"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1ea3bfc7-94ce-491c-ac44-a423a7b21c41"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4abbb88e-0008-40b4-a254-57fb759388f3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b8759e87-6883-414c-8ca1-e6f24b8063c6"));

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("27015e2f-e073-4358-8b06-09d2b444c609"), null, "User", "USER" },
                    { new Guid("933c8f2b-5187-40fb-b541-0ad7a3b3f5f8"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("95c3e755-53ee-4b73-a5d9-de4347ffe352"), null, "Moderator", "MODERATOR" },
                    { new Guid("d57685c3-470d-434e-b988-bf38f60d99a8"), null, "Member", "MEMBER" },
                    { new Guid("ea695385-d7c2-4909-8e3f-a1c1ff94185b"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("27015e2f-e073-4358-8b06-09d2b444c609"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("933c8f2b-5187-40fb-b541-0ad7a3b3f5f8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("95c3e755-53ee-4b73-a5d9-de4347ffe352"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d57685c3-470d-434e-b988-bf38f60d99a8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ea695385-d7c2-4909-8e3f-a1c1ff94185b"));

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Activities");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("149c675d-3f89-4cb0-b360-20b7e5c46d2d"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("1bac2dbe-31da-4957-bedb-472c8c060563"), null, "User", "USER" },
                    { new Guid("1ea3bfc7-94ce-491c-ac44-a423a7b21c41"), null, "Member", "MEMBER" },
                    { new Guid("4abbb88e-0008-40b4-a254-57fb759388f3"), null, "Admin", "ADMIN" },
                    { new Guid("b8759e87-6883-414c-8ca1-e6f24b8063c6"), null, "Moderator", "MODERATOR" }
                });
        }
    }
}
