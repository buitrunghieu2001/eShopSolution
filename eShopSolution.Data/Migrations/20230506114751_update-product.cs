using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b36ef46-2671-4671-8f0a-bbc26bff2100", "AQAAAAIAAYagAAAAEKv4JFx8pKOFaA+EIRZynmaq16fUs7baIO8I0JStTwJfuTiwbc4Yc7RKNtmoYEH03g==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "IsFeatured" },
                values: new object[] { new DateTime(2023, 5, 6, 18, 47, 50, 797, DateTimeKind.Local).AddTicks(3841), true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

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
                columns: new[] { "DateCreated", "IsFeatured" },
                values: new object[] { new DateTime(2023, 4, 24, 20, 20, 33, 942, DateTimeKind.Local).AddTicks(9721), null });
        }
    }
}
