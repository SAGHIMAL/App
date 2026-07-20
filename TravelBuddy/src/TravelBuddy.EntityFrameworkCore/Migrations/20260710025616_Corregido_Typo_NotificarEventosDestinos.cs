using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBuddy.Migrations
{
    /// <inheritdoc />
    public partial class Corregido_Typo_NotificarEventosDestinos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotiificarEventosDestinos",
                table: "AppPreferenciasNotificaciones",
                newName: "NotificarEventosDestinos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificarEventosDestinos",
                table: "AppPreferenciasNotificaciones",
                newName: "NotiificarEventosDestinos");
        }
    }
}
