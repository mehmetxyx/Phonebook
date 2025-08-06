using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenContactEntityAndUpdateContactDetailEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "contact_details",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "contact_details",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "ix_contacts_id",
                table: "contacts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contact_details_contact_id_id",
                table: "contact_details",
                columns: new[] { "contact_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contact_details_id",
                table: "contact_details",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details",
                column: "contact_id",
                principalTable: "contacts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_details_contacts_contact_id",
                table: "contact_details");

            migrationBuilder.DropIndex(
                name: "ix_contacts_id",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_contact_details_contact_id_id",
                table: "contact_details");

            migrationBuilder.DropIndex(
                name: "ix_contact_details_id",
                table: "contact_details");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "contact_details",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "contact_details",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
