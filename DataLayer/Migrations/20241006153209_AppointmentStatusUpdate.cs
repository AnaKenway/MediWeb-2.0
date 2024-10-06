using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentStatusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_approved",
                table: "appointment");

            migrationBuilder.AddColumn<int>(
                name: "appointment_status",
                table: "appointment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appointment_status",
                table: "appointment");

            migrationBuilder.AddColumn<BitArray>(
                name: "is_approved",
                table: "appointment",
                type: "bit(1)",
                nullable: false,
                defaultValueSql: "(0)::bit(1)");
        }
    }
}
