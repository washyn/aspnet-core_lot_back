using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class adedCOmisionRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.AddColumn<Guid>(
                name: "ComisionId",
                table: "Sorteo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                columns: new[] { "DocenteId", "RolId", "ComisionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "ComisionId",
                table: "Sorteo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                columns: new[] { "DocenteId", "RolId" });
        }
    }
}
