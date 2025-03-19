using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzureServiceBus.Migrations
{
    /// <inheritdoc />
    public partial class createDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    TransaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiposTransacciones = table.Column<int>(type: "int", nullable: false),
                    CuentaDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetallesAdicionales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProcesamiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaNotificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.TransaccionId);
                });

            migrationBuilder.CreateTable(
                name: "EventosTransacciones",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    TiposEventos = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosTransacciones", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_EventosTransacciones_Transacciones_TransaccionId",
                        column: x => x.TransaccionId,
                        principalTable: "Transacciones",
                        principalColumn: "TransaccionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    NotificacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    EmailCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.NotificacionId);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Transacciones_TransaccionId",
                        column: x => x.TransaccionId,
                        principalTable: "Transacciones",
                        principalColumn: "TransaccionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Transacciones",
                columns: new[] { "TransaccionId", "CuentaDestino", "DetallesAdicionales", "Estado", "FechaCreacion", "FechaNotificacion", "FechaProcesamiento", "Monto", "TiposTransacciones" },
                values: new object[,]
                {
                    { 1, "1234-5678-9012-3456", "Compra en tienda online", 0, new DateTime(2025, 3, 19, 0, 10, 46, 106, DateTimeKind.Local).AddTicks(6359), null, null, 100.50m, 0 },
                    { 2, "ES9121000418450200051332", "Pago de alquiler", 1, new DateTime(2025, 3, 19, 0, 10, 46, 108, DateTimeKind.Local).AddTicks(3808), null, new DateTime(2025, 3, 19, 0, 15, 46, 108, DateTimeKind.Local).AddTicks(3828), 250.75m, 1 },
                    { 3, "9876-5432-1098-7654", "Suscripción mensual", 2, new DateTime(2025, 3, 19, 0, 10, 46, 108, DateTimeKind.Local).AddTicks(6473), null, new DateTime(2025, 3, 19, 0, 13, 46, 108, DateTimeKind.Local).AddTicks(6499), 50.00m, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosTransacciones_TransaccionId",
                table: "EventosTransacciones",
                column: "TransaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_TransaccionId",
                table: "Notificaciones",
                column: "TransaccionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventosTransacciones");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Transacciones");
        }
    }
}
