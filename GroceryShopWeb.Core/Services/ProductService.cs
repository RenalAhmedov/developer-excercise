using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.ViewModels.Product;
using GroceryShopWeb.Infrastructure.Data;
using GroceryShopWeb.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopWeb.Core.Services
{
    public class ProductService : IProductService
    {
        private GroceryShopDbContext context;

        public ProductService(GroceryShopDbContext _context)
        {
            this.context = _context;
        }

        /// <summary>
        /// Adds Product to the database if it doesn't exist
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddProductAsync(AddProductViewModel model)
        {
            if(this.context.Products.Any(p => p.Name == model.Name))
            {
                return;
            }

            var product = new Product()
            {
                Name = model.Name.ToLower(),
                Price = model.Price
            };

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        public async Task<string> GetAllProductNamesAsync()
        {
            var names = await this.context.Products.Select(p => p.Name).ToArrayAsync();

            return string.Join(", ", names);
        }

        /// <summary>
        /// Gets all Products from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            return await this.context.Products
                .Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                }).ToListAsync();
        }

        /// <summary>
        /// Updates product's price
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="price">New Price</param>
        /// <returns></returns>
        public async Task UpdatePriceAsync(int id, double price)
        {
            if(price < 1 || price > 1000)
            {
                return;
            }

            var product = await this.context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            if(product == null)
            {
                return;
            }

            product.Price = price;

            await this.context.SaveChangesAsync();
        }
    }
}
