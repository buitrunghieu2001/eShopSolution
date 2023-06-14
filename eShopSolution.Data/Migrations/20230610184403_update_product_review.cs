using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_product_review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c0a02b52-2d8a-40d4-b0ba-35abd0b3047b", "AQAAAAIAAYagAAAAEGqAdyqRnQoYcIAqLNa1HbiXaqQFlgozAqU3gEDmAHV9JgAIDsIKxtdRoFUphyBTZQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 11, 1, 44, 2, 738, DateTimeKind.Local).AddTicks(3481));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "ProductReviews",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d81693d8-af89-4ce4-8abe-70244aeaf9d7", "AQAAAAIAAYagAAAAEFksFTa1etekCNzQI/PKZePUe7sJJ45l7luAA61A8PYyJMl3L9inrdW0T0FvtYqOuQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 5, 31, 13, 47, 15, 755, DateTimeKind.Local).AddTicks(9069));
        }
    }
}
