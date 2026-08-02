using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class initialCreatev6_add_customer_balance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PaymentIntent",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "PaymentIntent",
                newName: "channel");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "PaymentIntent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "channel",
                table: "PaymentIntent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "LedgerAccount",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "customerId",
                table: "LedgerAccount",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "LedgerAccount");

            migrationBuilder.DropColumn(
                name: "customerId",
                table: "LedgerAccount");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "PaymentIntent",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "channel",
                table: "PaymentIntent",
                newName: "Channel");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaymentIntent",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Channel",
                table: "PaymentIntent",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
