using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CambioTipoProcesador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcesadorPagos_TipoProcesadorPagos_TipoId",
                table: "ProcesadorPagos");

            migrationBuilder.DropTable(
                name: "TipoProcesadorPagos");

            migrationBuilder.DropIndex(
                name: "IX_ProcesadorPagos_TipoId",
                table: "ProcesadorPagos");

            migrationBuilder.AlterColumn<string>(
                name: "Metodo",
                table: "ProcesadorPagos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "ProcesadorPagos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "ProcesadorPagos");

            migrationBuilder.AlterColumn<string>(
                name: "Metodo",
                table: "ProcesadorPagos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TipoProcesadorPagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProcesadorPagos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcesadorPagos_TipoId",
                table: "ProcesadorPagos",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcesadorPagos_TipoProcesadorPagos_TipoId",
                table: "ProcesadorPagos",
                column: "TipoId",
                principalTable: "TipoProcesadorPagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
