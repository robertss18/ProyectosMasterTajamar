using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAzure.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventosTransaccion",
                keyColumn: "EventoId",
                keyValue: 1,
                column: "FechaEvento",
                value: new DateTime(2024, 3, 18, 12, 35, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Notificaciones",
                keyColumn: "NotificacionId",
                keyValue: 1,
                column: "FechaEnvio",
                value: new DateTime(2024, 3, 18, 12, 40, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 3, 18, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaNotificacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2024, 3, 18, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 18, 12, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 18, 12, 35, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventosTransaccion",
                keyColumn: "EventoId",
                keyValue: 1,
                column: "FechaEvento",
                value: new DateTime(2025, 3, 18, 23, 2, 56, 166, DateTimeKind.Utc).AddTicks(3967));

            migrationBuilder.UpdateData(
                table: "Notificaciones",
                keyColumn: "NotificacionId",
                keyValue: 1,
                column: "FechaEnvio",
                value: new DateTime(2025, 3, 18, 23, 2, 56, 166, DateTimeKind.Utc).AddTicks(5485));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7017));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaNotificacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7296), new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7807), new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7297) });
        }
    }
}
