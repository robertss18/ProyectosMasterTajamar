using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoAzure.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
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
                    TipoTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetallesAdicionales = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProcesamiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaNotificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.TransaccionId);
                });

            migrationBuilder.CreateTable(
                name: "EventosTransaccion",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    TipoEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosTransaccion", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_EventosTransaccion_Transacciones_TransaccionId",
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
                    EstadoNotificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                columns: new[] { "TransaccionId", "CuentaDestino", "DetallesAdicionales", "Estado", "FechaCreacion", "FechaNotificacion", "FechaProcesamiento", "Monto", "TipoTransaccion" },
                values: new object[,]
                {
                    { 1, "1234567890123456", "Compra en línea", "Pendiente", new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7017), null, null, 100.50m, "PagoConTarjeta" },
                    { 2, "9876543210123456", "Pago de renta", "Exitosa", new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7296), new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7807), new DateTime(2025, 3, 18, 23, 2, 56, 165, DateTimeKind.Utc).AddTicks(7297), 500.00m, "TransferenciaBancaria" }
                });

            migrationBuilder.InsertData(
                table: "EventosTransaccion",
                columns: new[] { "EventoId", "Descripcion", "FechaEvento", "TipoEvento", "TransaccionId" },
                values: new object[] { 1, "La transacción fue procesada con éxito", new DateTime(2025, 3, 18, 23, 2, 56, 166, DateTimeKind.Utc).AddTicks(3967), "Transacción aprobada", 2 });

            migrationBuilder.InsertData(
                table: "Notificaciones",
                columns: new[] { "NotificacionId", "EmailCliente", "EstadoNotificacion", "FechaEnvio", "TransaccionId" },
                values: new object[] { 1, "cliente@example.com", "Enviada", new DateTime(2025, 3, 18, 23, 2, 56, 166, DateTimeKind.Utc).AddTicks(5485), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_EventosTransaccion_TransaccionId",
                table: "EventosTransaccion",
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
                name: "EventosTransaccion");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Transacciones");
        }
    }
}
