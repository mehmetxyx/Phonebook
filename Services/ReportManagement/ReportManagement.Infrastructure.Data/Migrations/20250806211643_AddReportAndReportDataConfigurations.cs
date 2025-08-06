using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReportAndReportDataConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "reports",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "ix_reports_id",
                table: "reports",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_report_data_reports_report_id",
                table: "report_data",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_report_data_reports_report_id",
                table: "report_data");

            migrationBuilder.DropIndex(
                name: "ix_reports_id",
                table: "reports");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "reports",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
