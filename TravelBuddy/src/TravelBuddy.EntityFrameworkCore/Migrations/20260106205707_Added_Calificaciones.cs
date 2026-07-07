using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBuddy.Migrations
{
    /// <inheritdoc />
    public partial class Added_Calificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Valoracion",
                table: "AppExperiencias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valoracion",
                table: "AppExperiencias");
        }
    }
}
