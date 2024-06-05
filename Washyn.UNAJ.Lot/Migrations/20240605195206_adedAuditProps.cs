using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class adedAuditProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Sorteo",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sorteo",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "Sorteo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Docentes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Docentes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "Docentes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Sorteo");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Docentes");
        }
    }
}
