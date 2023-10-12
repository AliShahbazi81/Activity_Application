using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActivityApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RelationBetweenAttendeesAndActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ActivityAttendees",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsHost = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAttendees", x => new { x.ActivityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ActivityAttendees_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityAttendees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAttendees_UserId",
                table: "ActivityAttendees",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAttendees");

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
                    { new Guid("10e7aa15-4fc8-4445-bcbb-d4949f1b3862"), null, "Moderator", "MODERATOR" },
                    { new Guid("2f802987-9a4c-4866-9ff7-961806a681df"), null, "Member", "MEMBER" },
                    { new Guid("668ed48a-934d-4be2-bf0f-36dd0a499b58"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("c6437079-8062-444d-b36b-1e8ed64a7595"), null, "User", "USER" },
                    { new Guid("defe094e-41ed-422e-a89b-6e628cc11126"), null, "Admin", "ADMIN" }
                });
        }
    }
}
