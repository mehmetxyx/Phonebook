using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadingDeletionForContactEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details");

            migrationBuilder.AddForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details",
                column: "contact_id",
                principalTable: "contacts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details");

            migrationBuilder.AddForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details",
                column: "contact_id",
                principalTable: "contacts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
