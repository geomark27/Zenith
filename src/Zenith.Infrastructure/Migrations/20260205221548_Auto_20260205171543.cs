using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Auto_20260205171543 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "IsActive", "Name", "Phone", "Subdomain", "TaxId", "UpdatedAt" },
                values: new object[] { 1, "GUAYAQUIL", new DateTime(2026, 2, 5, 17, 8, 31, 0, DateTimeKind.Unspecified), "azentic@sys.com", true, "AZENTIC SYS", "0968319032", "azenticsys.com", "0953331675001", new DateTime(2026, 2, 5, 17, 8, 34, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "TenantId", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 2, 5, 17, 8, 31, 0, DateTimeKind.Unspecified), "admin@azenticsys.com", "Admin", true, "User", "CHANGE_ME", "ADMIN", 1, new DateTime(2026, 2, 5, 17, 8, 34, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
