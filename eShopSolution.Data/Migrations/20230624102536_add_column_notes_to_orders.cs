using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_column_notes_to_orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "32c198b5-e57d-4c47-8d05-087ee2acf80b", "AQAAAAIAAYagAAAAEC3EtPluufQgneq1VHW3x8dbOHetarF/STyANFgNsohFwCHBKThxaIS6oulVA/Aa3w==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 6, 24, 17, 25, 35, 703, DateTimeKind.Local).AddTicks(3511));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Orders");

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
    }
}
