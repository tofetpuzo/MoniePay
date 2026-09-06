using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class TransactionLedgerEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_LedgerEntries_LedgerEntriesId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_LedgerEntriesId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "LedgerEntriesId",
                table: "Transaction");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "LedgerEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transaction_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Transaction""
                ALTER COLUMN ""Status"" DROP DEFAULT,
                ALTER COLUMN ""Status"" TYPE integer
                    USING COALESCE(NULLIF(""Status"", '')::integer, 32),
                ALTER COLUMN ""Status"" SET NOT NULL;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transaction_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "LedgerEntries");

            migrationBuilder.AddColumn<Guid>(
                name: "LedgerEntriesId",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_LedgerEntriesId",
                table: "Transaction",
                column: "LedgerEntriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_LedgerEntries_LedgerEntriesId",
                table: "Transaction",
                column: "LedgerEntriesId",
                principalTable: "LedgerEntries",
                principalColumn: "Id");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Transaction""
                ALTER COLUMN ""Status"" TYPE text USING ""Status""::text,
                ALTER COLUMN ""Status"" SET NOT NULL;
            ");
        }
    }
}
