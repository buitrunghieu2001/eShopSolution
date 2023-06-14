using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class home_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28fca1ba-0d8c-4b48-b56a-4c70d60140c1", "AQAAAAIAAYagAAAAEAg7ad9MYOdTnTsmREfabzmcslXZKH6qBioHah5tohT7RLxgOqAll3v/rQe5Zzuqyg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 24, 20, 20, 33, 942, DateTimeKind.Local).AddTicks(9721));

            migrationBuilder.InsertData(
                table: "Slides",
                columns: new[] { "Id", "Description", "Image", "Name", "Price", "SortOrder", "Status", "Url" },
                values: new object[,]
                {
                    { 1, "Work Desk Surface Studio 2023", "images/slider/2.jpg", "Black Friday", 824.0, 1, 1, "shop-left-sidebar.html" },
                    { 2, "Work Desk Surface Studio 2023", "images/slider/1.jpg", "Black Friday", 824.0, 1, 1, "shop-left-sidebar.html" },
                    { 3, "Phantom 4 Pro+ Obsidian", "images/slider/3.jpg", "-10% Off", 1849.0, 1, 1, "shop-left-sidebar.html" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ff530d9c-67b3-4b37-820d-6a2fb5eb736a", "AQAAAAIAAYagAAAAEBCYiK9GBIe4DEi0kLkxGSf8F7++/q1/fTbrXTHNDz3k+AEeNFmJBPYhWS8FPC2s9g==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 24, 20, 17, 13, 521, DateTimeKind.Local).AddTicks(131));
        }
    }
}
