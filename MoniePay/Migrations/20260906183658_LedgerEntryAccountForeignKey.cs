using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class LedgerEntryAccountForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_LedgerAccount_LedgerAccountId",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_LedgerAccountId",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "LedgerAccountId",
                table: "LedgerEntries");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntries",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_LedgerAccount_AccountId",
                table: "LedgerEntries",
                column: "AccountId",
                principalTable: "LedgerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_LedgerAccount_AccountId",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntries");

            migrationBuilder.AddColumn<Guid>(
                name: "LedgerAccountId",
                table: "LedgerEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_LedgerAccountId",
                table: "LedgerEntries",
                column: "LedgerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_LedgerAccount_LedgerAccountId",
                table: "LedgerEntries",
                column: "LedgerAccountId",
                principalTable: "LedgerAccount",
                principalColumn: "Id");
        }
    }
}
