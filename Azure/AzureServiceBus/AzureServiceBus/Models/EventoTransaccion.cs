using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzureServiceBus.Models
{
    public class EventoTransaccion
    {

        [Key]
        public int EventoId { get; set; }

        [Required]
        [ForeignKey("Transaccion")]
        public int TransaccionId { get; set; }
        public virtual Transaccion? Transaccion { get; set; }

        [Required]
        public TipoEvento TiposEventos { get; set; } // 'Transacción aprobada', 'Transacción fallida'

        public string? Descripcion { get; set; }

        [Required]
        public DateTime FechaEvento { get; set; }

    }

    public enum TipoEvento
    {
        TransaccionAprobada,
        TransaccionFallida,
    }

}
