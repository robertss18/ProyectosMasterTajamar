using SneakersServiceSNS.Models;

namespace SneakersServiceSNS.Data
{
    public class DbInitializer
    {

        public static void Seed(ApplicationDbContext context)
        {
            if (context.Products.Any())
                return; // Ya están los datos

            var products = new List<Product>
            {
                new Product { Name = "Nike Air Max 270", Price = 149.99m, Description = "Modern style with great comfort." },
                new Product { Name = "Adidas Yeezy Boost 350", Price = 220.00m, Description = "Futuristic design and ultra comfort." },
                new Product { Name = "Puma RS-X³", Price = 110.00m, Description = "Colorful and perfect for the street." },
                new Product { Name = "New Balance 550", Price = 130.00m, Description = "Retro look with a modern twist." },
                new Product { Name = "Nike Dunk Low", Price = 115.00m, Description = "Streetwear classic." },
                new Product { Name = "Adidas Forum Low", Price = 100.00m, Description = "Revives the 80s spirit." },
                new Product { Name = "Reebok Club C 85", Price = 90.00m, Description = "Simple and clean elegance." },
                new Product { Name = "Converse Run Star Hike", Price = 120.00m, Description = "Bold and modern design." }
            };


            context.Products.AddRange(products);
            context.SaveChanges();
        }

    }
}
