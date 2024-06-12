using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Washyn.UNAJ.Lot.Migrations
{
    /// <inheritdoc />
    public partial class adedDocumentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "Docentes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dni",
                table: "Docentes");
        }
    }
}
