using System.ComponentModel.DataAnnotations.Schema;

namespace SneakersServiceSNS.Models
{
    public class Order
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

    }
}
