using System.ComponentModel.DataAnnotations;

namespace GroceryShopWeb.Infrastructure.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public double Price { get; set; }
    }
}
