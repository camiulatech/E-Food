using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CambiosTiqueteDescuento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                table: "TiqueteDescuentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "TiqueteDescuentos");
        }
    }
}
