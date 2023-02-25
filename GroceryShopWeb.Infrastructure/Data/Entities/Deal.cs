using System.ComponentModel.DataAnnotations;

namespace GroceryShopWeb.Infrastructure.Data.Entities
{
    public class Deal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Arguments { get; set; }
    }
}
