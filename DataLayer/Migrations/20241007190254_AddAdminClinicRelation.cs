using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminClinicRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "clinic_id",
                table: "admin",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_admin_clinic_id",
                table: "admin",
                column: "clinic_id");

            migrationBuilder.AddForeignKey(
                name: "admin_clinic_fkey",
                table: "admin",
                column: "clinic_id",
                principalTable: "clinic",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "admin_clinic_fkey",
                table: "admin");

            migrationBuilder.DropIndex(
                name: "IX_admin_clinic_id",
                table: "admin");

            migrationBuilder.DropColumn(
                name: "clinic_id",
                table: "admin");
        }
    }
}
