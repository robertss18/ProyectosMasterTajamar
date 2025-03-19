using AzureServiceBus.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureServiceBus.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<EventoTransaccion> EventosTransacciones { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Especificar precisión de Monto
            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);

            // Insertar datos estáticos
            modelBuilder.Entity<Transaccion>().HasData(
                new Transaccion
                {
                    TransaccionId = 1,
                    Monto = 100.50m,
                    TiposTransacciones = TipoTransaccion.PagoConTarjeta,
                    CuentaDestino = "1234-5678-9012-3456",
                    DetallesAdicionales = "Compra en tienda online",
                    Estado = EstadoTransaccion.Pendiente,
                    FechaCreacion = new DateTime(2024, 3, 15, 10, 30, 0)
                },
                new Transaccion
                {
                    TransaccionId = 2,
                    Monto = 250.75m,
                    TiposTransacciones = TipoTransaccion.TransferenciaBancaria,
                    CuentaDestino = "ES9121000418450200051332",
                    DetallesAdicionales = "Pago de alquiler",
                    Estado = EstadoTransaccion.Exitosa,
                    FechaCreacion = new DateTime(2024, 3, 15, 11, 0, 0),
                    FechaProcesamiento = new DateTime(2024, 3, 15, 11, 5, 0)
                },
                new Transaccion
                {
                    TransaccionId = 3,
                    Monto = 50.00m,
                    TiposTransacciones = TipoTransaccion.PagoConTarjeta,
                    CuentaDestino = "9876-5432-1098-7654",
                    DetallesAdicionales = "Suscripción mensual",
                    Estado = EstadoTransaccion.Fallida,
                    FechaCreacion = new DateTime(2024, 3, 15, 12, 15, 0),
                    FechaProcesamiento = new DateTime(2024, 3, 15, 12, 18, 0)
                }
            );
        }
    }
}
