using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActivityApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("391cf023-cceb-4a3b-84f3-812eb7a5cb16"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6fd46975-b5bb-4213-9e07-147e65112f10"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7ff6586b-c0b2-48c5-927e-1b4d94b0f6d1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ac3ee2bb-6d54-45a3-b268-edbc9b682078"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b92bbdbb-1cf3-4f74-b2fb-c0a77955f1c4"));

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10e7aa15-4fc8-4445-bcbb-d4949f1b3862"), null, "Moderator", "MODERATOR" },
                    { new Guid("2f802987-9a4c-4866-9ff7-961806a681df"), null, "Member", "MEMBER" },
                    { new Guid("668ed48a-934d-4be2-bf0f-36dd0a499b58"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("c6437079-8062-444d-b36b-1e8ed64a7595"), null, "User", "USER" },
                    { new Guid("defe094e-41ed-422e-a89b-6e628cc11126"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10e7aa15-4fc8-4445-bcbb-d4949f1b3862"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2f802987-9a4c-4866-9ff7-961806a681df"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("668ed48a-934d-4be2-bf0f-36dd0a499b58"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c6437079-8062-444d-b36b-1e8ed64a7595"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("defe094e-41ed-422e-a89b-6e628cc11126"));

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("391cf023-cceb-4a3b-84f3-812eb7a5cb16"), null, "Member", "MEMBER" },
                    { new Guid("6fd46975-b5bb-4213-9e07-147e65112f10"), null, "Moderator", "MODERATOR" },
                    { new Guid("7ff6586b-c0b2-48c5-927e-1b4d94b0f6d1"), null, "Admin", "ADMIN" },
                    { new Guid("ac3ee2bb-6d54-45a3-b268-edbc9b682078"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("b92bbdbb-1cf3-4f74-b2fb-c0a77955f1c4"), null, "User", "USER" }
                });
        }
    }
}
