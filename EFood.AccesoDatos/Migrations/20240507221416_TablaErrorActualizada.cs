using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class TablaErrorActualizada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Errors",
                newName: "Hora");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Errors",
                newName: "Mensaje");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Errors",
                newName: "Fecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mensaje",
                table: "Errors",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "Errors",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Errors",
                newName: "Date");
        }
    }
}
