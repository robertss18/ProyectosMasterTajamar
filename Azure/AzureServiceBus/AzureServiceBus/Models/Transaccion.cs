using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureServiceBus.Models
{
    public class Transaccion
    {
        [Key]
        public int TransaccionId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Especifica la precisión y escala
        public decimal Monto { get; set; }

        [Required]
        public TipoTransaccion TiposTransacciones { get; set; } // 'PagoConTarjeta' o 'TransferenciaBancaria'

        [Required]
        public string? CuentaDestino { get; set; }

        public string? DetallesAdicionales { get; set; }

        [Required]
        public EstadoTransaccion Estado { get; set; } // 'Pendiente', 'Exitosa', 'Fallida'

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaProcesamiento { get; set; }

        public DateTime? FechaNotificacion { get; set; }

        // Relaciones
        public virtual ICollection<EventoTransaccion>? Eventos { get; set; }
        public virtual Notificacion? Notificacion { get; set; }
    }

    // Enums deben estar fuera de la clase (aunque pueden estar dentro del mismo archivo)
    public enum TipoTransaccion
    {
        PagoConTarjeta,
        TransferenciaBancaria
    }

    public enum EstadoTransaccion
    {
        Pendiente,
        Exitosa,
        Fallida
    }
}
