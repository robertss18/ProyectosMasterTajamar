using System.ComponentModel.DataAnnotations;

namespace ProyectoAzure.Models {
    public class ColaTransaccionesNoEntregadas {        
        public int TransaccionId { get; set; }
        public string Error { get; set; } = string.Empty;
        public DateTime FechaError { get; set; }
    }
}
