using GroceryShopWeb.Core.ViewModels.Product;

namespace GroceryShopWeb.Core.Contracts
{
    public interface IOrderService
    {
        /// <summary>
        /// Process the order based on the input items.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Order's total cost</returns>
        Task<string> ProcessOrderAsync(OrderViewModel model);
    }
}
