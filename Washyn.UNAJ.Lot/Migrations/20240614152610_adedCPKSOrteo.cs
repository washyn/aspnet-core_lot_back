using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class adedCPKSOrteo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Sorteo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                columns: new[] { "RolId", "DocenteId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Sorteo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sorteo",
                table: "Sorteo",
                column: "Id");
        }
    }
}
