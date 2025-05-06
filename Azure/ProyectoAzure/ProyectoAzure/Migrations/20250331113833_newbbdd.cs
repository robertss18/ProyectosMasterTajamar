using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAzure.Migrations
{
    /// <inheritdoc />
    public partial class newbbdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                column: "Estado",
                value: "Exitoso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                column: "Estado",
                value: "Exitosa");
        }
    }
}
