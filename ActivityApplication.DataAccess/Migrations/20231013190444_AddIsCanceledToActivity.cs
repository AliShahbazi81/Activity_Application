using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActivityApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCanceledToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("078aab91-9970-405c-8783-873bc56103c1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("40cbaf05-8cf2-4644-a20a-0c92449e064c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7aa59a1a-47bd-4238-a4f3-f652c3122366"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("de5e4388-2cb4-4380-b819-024f0248db80"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ecc94fba-45f3-4708-b37b-2fa9c855b202"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("078aab91-9970-405c-8783-873bc56103c1"), null, "Member", "MEMBER" },
                    { new Guid("40cbaf05-8cf2-4644-a20a-0c92449e064c"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("7aa59a1a-47bd-4238-a4f3-f652c3122366"), null, "User", "USER" },
                    { new Guid("de5e4388-2cb4-4380-b819-024f0248db80"), null, "Admin", "ADMIN" },
                    { new Guid("ecc94fba-45f3-4708-b37b-2fa9c855b202"), null, "Moderator", "MODERATOR" }
                });
        }
    }
}
