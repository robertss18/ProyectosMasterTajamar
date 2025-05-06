
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoAzure.Models {
    public class EventoTransaccion
    {
        [Key]
        public int EventoId { get; set; }
        public int TransaccionId { get; set; }
        public string? TipoEvento { get; set; } // Transacción aprobada, Transacción fallida
        public string? Descripcion { get; set; }
        public DateTime FechaEvento { get; set; }
        [JsonIgnore]        
        public Transaccion? Transaccion { get; set; }
    }
}
