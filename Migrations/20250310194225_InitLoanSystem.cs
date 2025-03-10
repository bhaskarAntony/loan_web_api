using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitLoanSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "Loans",
                newName: "RejectedTime");

            migrationBuilder.RenameColumn(
                name: "AppliedDate",
                table: "Loans",
                newName: "AppliedTime");

            migrationBuilder.RenameColumn(
                name: "IdNumber",
                table: "Kycs",
                newName: "PanNumber");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Kycs",
                newName: "FullName");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedTime",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlySalary",
                table: "Loans",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AadharNumber",
                table: "Kycs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedTime",
                table: "Kycs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedTime",
                table: "Kycs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Kycs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Kycs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Kycs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedTime",
                table: "Kycs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCards_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_LoanId",
                table: "CreditCards",
                column: "LoanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_UserId",
                table: "CreditCards",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropColumn(
                name: "ApprovedTime",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "MonthlySalary",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AadharNumber",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "AppliedTime",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "ApprovedTime",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Kycs");

            migrationBuilder.DropColumn(
                name: "RejectedTime",
                table: "Kycs");

            migrationBuilder.RenameColumn(
                name: "RejectedTime",
                table: "Loans",
                newName: "ApprovedDate");

            migrationBuilder.RenameColumn(
                name: "AppliedTime",
                table: "Loans",
                newName: "AppliedDate");

            migrationBuilder.RenameColumn(
                name: "PanNumber",
                table: "Kycs",
                newName: "IdNumber");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Kycs",
                newName: "DocumentType");
        }
    }
}
