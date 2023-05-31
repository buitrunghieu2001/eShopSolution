using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_appUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 1);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUsers");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "347a6dc0-8918-4a42-bf2d-d34be1db36da", "AQAAAAIAAYagAAAAEEPpC0yk+y75QYI+Xh100zwTsWlC8RGDJtJJ8sWWb8qyOUQSW6Lyas6tGUts2ub9Xw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 5, 29, 19, 40, 22, 985, DateTimeKind.Local).AddTicks(8316));
        }
    }
}
