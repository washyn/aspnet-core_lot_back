using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Participantes");

            migrationBuilder.RenameColumn(
                name: "CursoId",
                table: "Participantes",
                newName: "ComisionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes",
                columns: new[] { "DocenteId", "ComisionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes");

            migrationBuilder.RenameColumn(
                name: "ComisionId",
                table: "Participantes",
                newName: "CursoId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Participantes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes",
                column: "Id");
        }
    }
}
