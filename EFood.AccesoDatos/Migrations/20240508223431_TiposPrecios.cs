using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class TiposPrecios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Monto",
                table: "TipoPrecios");

            migrationBuilder.AddColumn<float>(
                name: "Cambio",
                table: "TipoPrecios",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<decimal>(
                name: "Monto",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cambio",
                table: "TipoPrecios");

            migrationBuilder.DropColumn(
                name: "Monto",
                table: "Productos");

            migrationBuilder.AddColumn<decimal>(
                name: "Monto",
                table: "TipoPrecios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
