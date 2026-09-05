using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// Hand-written. The scaffolder emitted only RenameColumn("Channel" -> "channel"),
    /// which would have failed: the database already has lowercase "channel", the
    /// snapshot was simply stale. The real drift was invisible to EF because model
    /// and snapshot agreed while the database did not:
    ///   - DestinationAccountNumber / DestinationAccountName were added to
    ///     PaymentIntents while the snapshot was ahead, so no migration created them.
    ///   - Status is "status" in the database but "Status" in the model.
    /// </remarks>
    public partial class FixPaymentIntentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationAccountNumber",
                table: "PaymentIntent",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestinationAccountName",
                table: "PaymentIntent",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "PaymentIntent",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PaymentIntent",
                newName: "status");

            migrationBuilder.DropColumn(
                name: "DestinationAccountName",
                table: "PaymentIntent");

            migrationBuilder.DropColumn(
                name: "DestinationAccountNumber",
                table: "PaymentIntent");
        }
    }
}
