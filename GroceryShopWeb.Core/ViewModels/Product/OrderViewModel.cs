namespace GroceryShopWeb.Core.ViewModels.Product
{
    public class OrderViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

        public string Basket { get; set; } = null!;
        public bool TwoForThree { get; set; }
        public bool HalfPrice { get; set; }
    }
}
