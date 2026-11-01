using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class VaultModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vault",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CustodianId = table.Column<Guid>(type: "uuid", nullable: false),
                    LedgerAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vault", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vault_LedgerAccount_LedgerAccountId",
                        column: x => x.LedgerAccountId,
                        principalTable: "LedgerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vault_LedgerAccountId",
                table: "Vault",
                column: "LedgerAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vault_Name",
                table: "Vault",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vault");
        }
    }
}
