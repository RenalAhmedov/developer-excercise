using GroceryShopWeb.Core.ViewModels.Product;

namespace GroceryShopWeb.Core.Contracts
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all Products from the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();

        /// <summary>
        /// Updates product's price
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="price">New Price</param>
        /// <returns></returns>
        Task UpdatePriceAsync(int id, double price);

        /// <summary>
        /// Adds Product to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddProductAsync(AddProductViewModel model);

        /// <summary>
        /// Gets all product names as a single string separated by comas
        /// </summary>
        /// <returns></returns>
        Task<string> GetAllProductNamesAsync();
    }
}
