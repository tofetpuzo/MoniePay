using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class initialCreatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "userId",
                table: "Customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_userId",
                table: "Customers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_userId",
                table: "Customers",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_userId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_userId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Customers");
        }
    }
}
