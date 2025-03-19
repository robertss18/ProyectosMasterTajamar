namespace AzureServiceBus.Models
{
    public class ColaTransaccionesPendientes
    {

        public int TransaccionId { get; set; }
        public decimal Monto { get; set; }
        public string? TipoTransaccion { get; set; }
        public string? Estado { get; set; }

    }
}
