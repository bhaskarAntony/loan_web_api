using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixCreditCardSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Users_UserId",
                table: "CreditCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_CreditCards_CreditCardCardId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_CreditCardCardId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "CreditCardCardId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CreditCards");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "CreditCards",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CreditCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ExpiryDate",
                table: "CreditCards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<decimal>(
                name: "ApprovedAmount",
                table: "CreditCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "CreditCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_LoanId",
                table: "CreditCards",
                column: "LoanId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Loans_LoanId",
                table: "CreditCards",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Users_UserId",
                table: "CreditCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Loans_LoanId",
                table: "CreditCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Users_UserId",
                table: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_LoanId",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "ApprovedAmount",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "CreditCards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CreditCards",
                newName: "CardId");

            migrationBuilder.AddColumn<int>(
                name: "CreditCardCardId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CreditCards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "CreditCards",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CreditCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CreditCardCardId",
                table: "Loans",
                column: "CreditCardCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Users_UserId",
                table: "CreditCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_CreditCards_CreditCardCardId",
                table: "Loans",
                column: "CreditCardCardId",
                principalTable: "CreditCards",
                principalColumn: "CardId");
        }
    }
}
