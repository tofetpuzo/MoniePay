using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class FixBankLedgerAccountName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_LedgerAccount_LedgerAcountId",
                table: "BankAccount");

            migrationBuilder.RenameColumn(
                name: "LedgerAcountId",
                table: "BankAccount",
                newName: "LedgerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_LedgerAcountId",
                table: "BankAccount",
                newName: "IX_BankAccount_LedgerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_LedgerAccount_LedgerAccountId",
                table: "BankAccount",
                column: "LedgerAccountId",
                principalTable: "LedgerAccount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_LedgerAccount_LedgerAccountId",
                table: "BankAccount");

            migrationBuilder.RenameColumn(
                name: "LedgerAccountId",
                table: "BankAccount",
                newName: "LedgerAcountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_LedgerAccountId",
                table: "BankAccount",
                newName: "IX_BankAccount_LedgerAcountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_LedgerAccount_LedgerAcountId",
                table: "BankAccount",
                column: "LedgerAcountId",
                principalTable: "LedgerAccount",
                principalColumn: "Id");
        }
    }
}
