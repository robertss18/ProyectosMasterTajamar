using Microsoft.EntityFrameworkCore;
using ProyectoAzure.Models;

namespace ProyectoAzure.Data {
    public class AplicationDbContext : DbContext {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }

        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<EventoTransaccion> EventosTransaccion { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Transaccion>().HasKey(t => t.TransaccionId);
            modelBuilder.Entity<EventoTransaccion>().HasKey(e => e.EventoId);
            modelBuilder.Entity<Notificacion>().HasKey(n => n.NotificacionId);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<EventoTransaccion>()
                .HasOne(e => e.Transaccion)
                .WithMany(t => t.Eventos)
                .HasForeignKey(e => e.TransaccionId);

            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Transaccion)
                .WithOne(t => t.Notificacion)
                .HasForeignKey<Notificacion>(n => n.TransaccionId);

            // Datos de prueba (seeding) con valores fijos en vez de dinámicos
            modelBuilder.Entity<Transaccion>().HasData(
                new Transaccion { TransaccionId = 1, Monto = 100.50m, TipoTransaccion = "PagoConTarjeta", CuentaDestino = "1234567890123456", DetallesAdicionales = "Compra en línea", Estado = "Pendiente", FechaCreacion = new DateTime(2024, 03, 18, 12, 00, 00) },
                new Transaccion { TransaccionId = 2, Monto = 500.00m, TipoTransaccion = "TransferenciaBancaria", CuentaDestino = "9876543210123456", DetallesAdicionales = "Pago de renta", Estado = "Exitoso", FechaCreacion = new DateTime(2024, 03, 18, 12, 30, 00), FechaProcesamiento = new DateTime(2024, 03, 18, 12, 35, 00), FechaNotificacion = new DateTime(2024, 03, 18, 12, 40, 00) }
            );

            modelBuilder.Entity<EventoTransaccion>().HasData(
                new EventoTransaccion { EventoId = 1, TransaccionId = 2, TipoEvento = "Transacción aprobada", Descripcion = "La transacción fue procesada con éxito", FechaEvento = new DateTime(2024, 03, 18, 12, 35, 00) }
            );

            modelBuilder.Entity<Notificacion>().HasData(
                new Notificacion { NotificacionId = 1, TransaccionId = 2, EmailCliente = "cliente@example.com", EstadoNotificacion = "Enviada", FechaEnvio = new DateTime(2024, 03, 18, 12, 40, 00) }
            );
        }
    }
}