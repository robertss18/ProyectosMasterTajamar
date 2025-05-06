using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoAzure.Models {
    public class Notificacion {
        [Key]
        public int NotificacionId { get; set; }
        public int TransaccionId { get; set; }
        public string? EmailCliente { get; set; }
        public string? EstadoNotificacion { get; set; } // Enviada, Fallida
        public DateTime FechaEnvio { get; set; }
        [JsonIgnore]
        public Transaccion? Transaccion { get; set; }
    }
}
