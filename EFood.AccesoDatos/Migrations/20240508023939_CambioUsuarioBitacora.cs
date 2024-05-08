using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CambioUsuarioBitacora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bitacoras_AspNetUsers_UsuarioId",
                table: "Bitacoras");

            migrationBuilder.DropIndex(
                name: "IX_Bitacoras_UsuarioId",
                table: "Bitacoras");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Bitacoras");

            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "Bitacoras",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "Bitacoras");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Bitacoras",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_UsuarioId",
                table: "Bitacoras",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bitacoras_AspNetUsers_UsuarioId",
                table: "Bitacoras",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
