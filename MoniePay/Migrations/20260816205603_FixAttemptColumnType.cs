using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoniePay.Migrations
{
    /// <inheritdoc />
    public partial class FixAttemptColumnType : Migration
    {
        /// <inheritdoc />
        /// <remarks>
        /// Hand-written. Payments.Attempt was changed string -> int, but the model
        /// snapshot had already been updated to int, so the scaffolder produced an
        /// empty diff and the column stayed "text" in the database. Postgres has no
        /// implicit text -> integer cast, so the USING clause is required.
        /// </remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payment""
                ALTER COLUMN ""Attempt"" DROP DEFAULT,
                ALTER COLUMN ""Attempt"" TYPE integer
                    USING COALESCE(NULLIF(""Attempt"", '')::integer, 1),
                ALTER COLUMN ""Attempt"" SET NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payment""
                ALTER COLUMN ""Attempt"" TYPE text
                    USING ""Attempt""::text,
                ALTER COLUMN ""Attempt"" SET NOT NULL;
            ");
        }
    }
}
