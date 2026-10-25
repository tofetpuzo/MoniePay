using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class BankingFundingAndTransactionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentIntentId",
                table: "Transaction",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "InitiatedBy",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: false),
                    AccountName = table.Column<string>(type: "text", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    SortCode = table.Column<string>(type: "text", nullable: false),
                    SwiftCode = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LedgerAcountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_LedgerAccount_LedgerAcountId",
                        column: x => x.LedgerAcountId,
                        principalTable: "LedgerAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankFunding",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    BankReference = table.Column<string>(type: "text", nullable: false),
                    Narration = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RecordedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankFunding_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankFunding_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentIntentId",
                table: "Transaction",
                column: "PaymentIntentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Type_Reference",
                table: "Transaction",
                columns: new[] { "Type", "Reference" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_LedgerAcountId",
                table: "BankAccount",
                column: "LedgerAcountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_SortCode_AccountNumber",
                table: "BankAccount",
                columns: new[] { "SortCode", "AccountNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankFunding_BankAccountId",
                table: "BankFunding",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankFunding_BankReference",
                table: "BankFunding",
                column: "BankReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankFunding_TransactionId",
                table: "BankFunding",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankFunding");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PaymentIntentId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_Type_Reference",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "InitiatedBy",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentIntentId",
                table: "Transaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
