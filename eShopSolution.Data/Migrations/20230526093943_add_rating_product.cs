using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_rating_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "44f43d4a-7650-4dbc-a597-71a501143f28", "AQAAAAIAAYagAAAAEE8sAZ0udEglD/VZkT4z7XSErjVHq00nGeL+Azl/O4oEZrMaf+UELjYGmJPpkoMnIQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 5, 26, 16, 39, 43, 499, DateTimeKind.Local).AddTicks(6188));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "806ca3df-64f6-41fd-abc0-0a5d1212486d", "AQAAAAIAAYagAAAAEFJ2x8ImoqZA2Jrg9HQ0ar7TAgNN2huJR2qUBI4HcMdXo7eu7vf5VXqk5CsFeLUoSA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 5, 25, 0, 12, 15, 896, DateTimeKind.Local).AddTicks(8766));
        }
    }
}
