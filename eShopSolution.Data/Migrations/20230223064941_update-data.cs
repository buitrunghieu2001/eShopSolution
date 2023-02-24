using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9795044c-ed97-4195-98e1-f67eb97fc3d2", "AQAAAAIAAYagAAAAEBy6rAWaoETWNPNI0zoXSpLfIQ11TA1GZA6pyCbT+EeMLbwkiUWRqGyaXgVPI2dGXQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 23, 13, 49, 41, 536, DateTimeKind.Local).AddTicks(970));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dfe8cb77-15f7-4403-b3a0-9e180f0629a2", "AQAAAAIAAYagAAAAECnGZxJ8d2DKj+qV8fOjJ1XsvX3MOdSL0CGq6hs9jRZuWfRocvmF8pru0CxHvp9vjg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 23, 11, 27, 16, 591, DateTimeKind.Local).AddTicks(7319));
        }
    }
}
