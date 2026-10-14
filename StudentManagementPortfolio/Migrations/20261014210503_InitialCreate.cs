using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Department", "Email", "EnrollmentDate", "FullName", "PhoneNumber", "Status", "Year" },
                values: new object[,]
                {
                    { 1, "Computer Science", "mariam@example.com", new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mariam Hassan", "01000000001", 0, 3 },
                    { 2, "Information Systems", "omar@example.com", new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar Adel", "01000000002", 0, 2 },
                    { 3, "Artificial Intelligence", "nour@example.com", new DateTime(2022, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nour Ahmed", "01000000003", 1, 4 },
                    { 4, "Computer Science", "youssef@example.com", new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youssef Samir", "01000000004", 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
