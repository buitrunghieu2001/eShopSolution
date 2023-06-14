using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "440fc890-ca4c-448a-bced-3e7f7bdf4272", "AQAAAAIAAYagAAAAEMXzHuZT10rrxUsUn7fQQKfJt+HpXoxonf5Z+zH1FRLiAf5YnNIMXojClg7AeQQhhw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 10, 15, 34, 35, 613, DateTimeKind.Local).AddTicks(1001));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8f811782-a1c7-4cb2-9df9-03066eaf1cd0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "191a5cfb-9cc2-400b-b129-a865606f6065", "AQAAAAIAAYagAAAAEPnSUed1JNOn6WoyEUYVBCVeMWekqPdNERVlH5hb3p80hJNKUrlpTfJGKjis9WMd/g==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 4, 10, 15, 25, 21, 829, DateTimeKind.Local).AddTicks(5645));
        }
    }
}
