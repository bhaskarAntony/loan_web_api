using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLoansToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Kycs_KycId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_KycId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "KycId",
                table: "Loans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KycId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_KycId",
                table: "Loans",
                column: "KycId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Kycs_KycId",
                table: "Loans",
                column: "KycId",
                principalTable: "Kycs",
                principalColumn: "Id");
        }
    }
}
