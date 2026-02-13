using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Auto_20260205160616 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Payrolls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Payrolls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Catalogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Catalogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_CreatedById",
                table: "Payrolls",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_UpdatedById",
                table: "Payrolls",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UpdatedById",
                table: "Employees",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdatedById",
                table: "Departments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CreatedById",
                table: "Catalogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_UpdatedById",
                table: "Catalogs",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CreatedById",
                table: "Attendances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UpdatedById",
                table: "Attendances",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_TenantId",
                table: "Users",
                columns: new[] { "Email", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Users_CreatedById",
                table: "Attendances",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Users_UpdatedById",
                table: "Attendances",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Users_CreatedById",
                table: "Catalogs",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Users_UpdatedById",
                table: "Catalogs",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_CreatedById",
                table: "Departments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_UpdatedById",
                table: "Departments",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_CreatedById",
                table: "Employees",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UpdatedById",
                table: "Employees",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payrolls_Users_CreatedById",
                table: "Payrolls",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payrolls_Users_UpdatedById",
                table: "Payrolls",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Users_CreatedById",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Users_UpdatedById",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Users_CreatedById",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Users_UpdatedById",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_CreatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_UpdatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_CreatedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UpdatedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_Users_CreatedById",
                table: "Payrolls");

            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_Users_UpdatedById",
                table: "Payrolls");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Payrolls_CreatedById",
                table: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Payrolls_UpdatedById",
                table: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UpdatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UpdatedById",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CreatedById",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_UpdatedById",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_CreatedById",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_UpdatedById",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Attendances");
        }
    }
}
