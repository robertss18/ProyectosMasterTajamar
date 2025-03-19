using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureServiceBus.Migrations
{
    /// <inheritdoc />
    public partial class AjusteMontoYDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 3, 15, 10, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2024, 3, 15, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 15, 11, 5, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 15, 12, 18, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 3, 19, 0, 10, 46, 106, DateTimeKind.Local).AddTicks(6359));

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2025, 3, 19, 0, 10, 46, 108, DateTimeKind.Local).AddTicks(3808), new DateTime(2025, 3, 19, 0, 15, 46, 108, DateTimeKind.Local).AddTicks(3828) });

            migrationBuilder.UpdateData(
                table: "Transacciones",
                keyColumn: "TransaccionId",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "FechaProcesamiento" },
                values: new object[] { new DateTime(2025, 3, 19, 0, 10, 46, 108, DateTimeKind.Local).AddTicks(6473), new DateTime(2025, 3, 19, 0, 13, 46, 108, DateTimeKind.Local).AddTicks(6499) });
        }
    }
}
