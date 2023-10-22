using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActivityApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3ce94a01-2b3e-4c01-aead-c0a4e8ea3d1e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("764f2569-d4da-4d24-b006-3bafeb666ab3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9761a6ef-61f2-4178-b9f1-19fd57e7721a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bafc9b1a-b787-433a-8ac7-8076ac879f03"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f394e967-2177-4aba-88db-ec389958c79d"));

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("30f9dd1a-c231-44d2-bff7-dd286560f941"), null, "Moderator", "MODERATOR" },
                    { new Guid("677dcca9-28af-457f-8c34-6ad9d538c00b"), null, "BannedUser", "BANNED_USER" },
                    { new Guid("7dcbfd32-7c4d-42f2-88f0-e5dfaef4e007"), null, "User", "USER" },
                    { new Guid("d3a65b2d-ddc6-4b36-bb57-4f4f308234e4"), null, "Member", "MEMBER" },
                    { new Guid("f0f64481-6fe6-4e40-a46c-33185e0d3b5f"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ActivityId",
                table: "Comments",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("30f9dd1a-c231-44d2-bff7-dd286560f941"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("677dcca9-28af-457f-8c34-6ad9d538c00b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7dcbfd32-7c4d-42f2-88f0-e5dfaef4e007"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d3a65b2d-ddc6-4b36-bb57-4f4f308234e4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f0f64481-6fe6-4e40-a46c-33185e0d3b5f"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3ce94a01-2b3e-4c01-aead-c0a4e8ea3d1e"), null, "Admin", "ADMIN" },
                    { new Guid("764f2569-d4da-4d24-b006-3bafeb666ab3"), null, "Member", "MEMBER" },
                    { new Guid("9761a6ef-61f2-4178-b9f1-19fd57e7721a"), null, "User", "USER" },
                    { new Guid("bafc9b1a-b787-433a-8ac7-8076ac879f03"), null, "Moderator", "MODERATOR" },
                    { new Guid("f394e967-2177-4aba-88db-ec389958c79d"), null, "BannedUser", "BANNED_USER" }
                });
        }
    }
}
