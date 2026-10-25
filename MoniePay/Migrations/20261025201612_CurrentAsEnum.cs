using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class CurrentAsEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "LedgerAccount",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "accountType",
                table: "LedgerAccount",
                newName: "AccountType");

            var names = string.Join(",",
                Enum.GetNames(typeof(MoniePay.src.Currency)).Select(n => $"'{n}'"));

            foreach (var table in new[] { "LedgerAccount", "PaymentIntent" })
            {
                migrationBuilder.Sql($@"
                ALTER TABLE ""{table}""
                ALTER COLUMN ""Currency"" TYPE integer
                    USING array_position(ARRAY[{names}]::text[], ""Currency"") - 1;");
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "LedgerAccount",
                newName: "customerId");

            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "LedgerAccount",
                newName: "accountType");

            var names = string.Join(",",
                Enum.GetNames(typeof(MoniePay.src.Currency)).Select(n => $"'{n}'"));

            foreach (var table in new[] { "LedgerAccount", "PaymentIntent" })
            {
                migrationBuilder.Sql($@"ALTER TABLE ""{table}""
                ALTER COLUMN ""Currency"" TYPE text
                USING (ARRAY[{names}]::text[])[""Currency"" + 1];");
            }
        }
    }
}
