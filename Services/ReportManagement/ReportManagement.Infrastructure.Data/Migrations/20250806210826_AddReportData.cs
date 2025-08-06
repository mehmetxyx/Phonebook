using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReportData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    report_id = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    contact_count = table.Column<int>(type: "integer", nullable: false),
                    phone_number_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_data", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_report_data_id",
                table: "report_data",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_report_data_report_id_id",
                table: "report_data",
                columns: new[] { "report_id", "id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_data");
        }
    }
}
