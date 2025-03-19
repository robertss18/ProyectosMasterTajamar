using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzureServiceBus.Models
{
    public class Notificacion
    {

        [Key]
        public int NotificacionId { get; set; }

        [Required]
        [ForeignKey("Transaccion")]
        public int TransaccionId { get; set; }
        public virtual Transaccion Transaccion { get; set; }

        [Required]
        [EmailAddress]
        public string EmailCliente { get; set; }

        [Required]
        public EstadoNotificacion Estado { get; set; } // 'Enviada', 'Fallida'

        [Required]
        public DateTime FechaEnvio { get; set; }

    }

    public enum EstadoNotificacion
    {
        Enviada,
        Fallida
    }

}
