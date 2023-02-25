using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.ViewModels.Deal;
using GroceryShopWeb.Infrastructure.Data;
using GroceryShopWeb.Infrastructure.DataConstants;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopWeb.Core.Services
{
    public class DealService : IDealService
    {
        private readonly GroceryShopDbContext context;
        private readonly IProductService productService;

        public DealService(GroceryShopDbContext _context, IProductService _productService)
        {
            this.context = _context;
            this.productService = _productService;
        }

        /// <summary>
        /// Gets the two Deals from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DealViewModel>> GetAllDealsAsync()
        {
            return await this.context.Deals
                .Select(d => new DealViewModel()
                {
                    Id = d.Id,
                    Type = d.Type,
                    Arguments = d.Arguments
                }).ToListAsync();
        }

        /// <summary>
        /// Gets the 'Buy 1 get 1 for half price' deal from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<DealViewModel> GetHalfPriceDealAsync()
        {
            return await this.context.Deals
                .Where(d => d.Type == DealConstants.HalfPrice)
                .Select(d => new DealViewModel()
                {
                    Id = d.Id,
                    Type = d.Type,
                    Arguments = d.Arguments
                }).FirstAsync();
        }

        /// <summary>
        /// Gets the '2 for 3' deal from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<DealViewModel> GetTwoForThreeDealAsync()
        {
            return await this.context.Deals
                .Where(d => d.Type == DealConstants.TwoForThree)
                .Select(d => new DealViewModel()
                {
                    Id = d.Id,
                    Type = d.Type,
                    Arguments = d.Arguments
                }).FirstAsync();
        }

        /// <summary>
        /// Updates Deal from the Database
        /// If the input is not valid - the method returns and no changes are made to the database.
        /// </summary>
        /// <param name="id">Deal Id</param>
        /// <param name="arguments">Deal Arguments</param>
        /// <returns></returns>
        public async Task UpdateDealAsync(int id, string arguments)
        {
            var deal = await this.context.Deals.Where(d => d.Id == id).FirstOrDefaultAsync();

            if(deal == null)
            {
                return;
            }

            var input = arguments.Split(", ").Select(p => p.Trim());

            if(deal.Type == DealConstants.TwoForThree && input.Distinct().Count() != 3)
            {
                return;
            }

            if(deal.Type == DealConstants.HalfPrice && input.Count() != 1)
            {
                return;
            }

            var products = await this.productService.GetAllProductNamesAsync();

            var existingProducts = products.Split(", ");

            foreach (var p in input)
            {
                if (!existingProducts.Contains(p))
                {
                    return;
                }
            }

            deal.Arguments = arguments;

            await context.SaveChangesAsync();
        }
    }
}
