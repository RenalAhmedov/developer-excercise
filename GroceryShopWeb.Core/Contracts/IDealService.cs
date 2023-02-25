using GroceryShopWeb.Core.ViewModels.Deal;

namespace GroceryShopWeb.Core.Contracts
{
    public interface IDealService
    {
        /// <summary>
        /// Gets the two Deals from the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DealViewModel>> GetAllDealsAsync();

        /// <summary>
        /// Gets the 'Buy 1 get 1 for half price' deal from the database.
        /// </summary>
        /// <returns></returns>
        Task<DealViewModel> GetTwoForThreeDealAsync();

        /// <summary>
        /// Gets the '2 for 3' deal from the database.
        /// </summary>
        /// <returns></returns>
        Task<DealViewModel> GetHalfPriceDealAsync();

        /// <summary>
        /// Updates Deal from the Database
        /// </summary>
        /// <param name="id">Deal Id</param>
        /// <param name="arguments">Deal Arguments</param>
        /// <returns></returns>
        Task UpdateDealAsync(int id, string arguments);
    }
}
