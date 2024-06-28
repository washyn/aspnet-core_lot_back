using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class AdedParticipantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                columns: new[] { "DocenteId", "RolId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes",
                columns: new[] { "ComisionId", "DocenteId" });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                columns: new[] { "RolId", "DocenteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes",
                columns: new[] { "DocenteId", "ComisionId" });
        }
    }
}
