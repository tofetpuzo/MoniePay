using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate_acount_number_seq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AddColumn is hand-added. LedgerAccounts.AccountNumber was introduced
            // while the model snapshot was already ahead of the database, so the
            // scaffolder believed the column existed and emitted only the index -
            // which then failed with 42703: column "AccountNumber" does not exist.
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "LedgerAccount",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateSequence(
                name: "account_number_seq",
                startValue: 100000000000L);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccount_AccountNumber",
                table: "LedgerAccount",
                column: "AccountNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LedgerAccount_AccountNumber",
                table: "LedgerAccount");

            migrationBuilder.DropSequence(
                name: "account_number_seq");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "LedgerAccount");
        }
    }
}
