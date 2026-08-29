using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate_update_accounttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "LedgerAccount",
                newName: "accountType");

            migrationBuilder.AlterColumn<int>(
                name: "accountType",
                table: "LedgerAccount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "accountType",
                table: "LedgerAccount",
                newName: "AccountType");

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "LedgerAccount",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
