using System.ComponentModel.DataAnnotations;

namespace GroceryShopWeb.Core.ViewModels.Product
{
    public class AddProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }
    }
}
