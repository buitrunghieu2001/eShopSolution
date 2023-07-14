using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_table_orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipCommune",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipDistrict",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipProvince",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6b31fff3-7a7c-4198-b8e9-5989d9e74d86", "AQAAAAIAAYagAAAAEBxSfeh1M6yyvPjO2VjTkYDke6OHKLm9eNxFsbNJmVrWgQdKcccxRJ6YduvYWFXtQA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 24, 17, 8, 19, 987, DateTimeKind.Local).AddTicks(3504));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipCommune",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipDistrict",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipProvince",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0af73492-6558-4be7-8065-5ee5bfc4eb7b", "AQAAAAIAAYagAAAAEEeO7aCTQFYJnpTaBWeuUxaSHOShv71yLSwMttu8B+FepOy0yeK+uhcklap0EVcDrg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 24, 17, 1, 34, 794, DateTimeKind.Local).AddTicks(7088));
        }
    }
}
