using GroceryShopWeb.Core.Constants;
using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.ViewModels.Product;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data.Common;
using System.Text;

namespace GroceryShopWeb.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductService productService;
        private readonly IDealService dealService;

        public OrderService(IProductService _productService, IDealService _dealService)
        {
            this.productService = _productService;
            this.dealService = _dealService;
        }

        /// <summary>
        /// Process the order based on the input items.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Order's total cost</returns>
        public async Task<string> ProcessOrderAsync(OrderViewModel model)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(model.Basket))
            {
                sb.AppendLine(OutputConstants.EMPTY_BASKET);
                return sb.ToString().TrimEnd();
            }
            
            var dbProducts = await this.productService.GetAllProductsAsync();
            var tftDeal = await this.dealService.GetTwoForThreeDealAsync();
            var hpDeal = await this.dealService.GetHalfPriceDealAsync();

            //filters for invalid products
            var filteredProducts = model.Basket.Split(", ").Where(p => dbProducts.Any(dbp => dbp.Name == p.ToLower())).Select(x => x.Trim()).ToList();
            var invalidProducts = model.Basket.Split(", ").Where(p => !dbProducts.Any(dbp => dbp.Name == p.ToLower())).Select(x => x.Trim()).ToList();

            invalidProducts.ForEach(p => sb.AppendLine(string.Format(OutputConstants.INVALID_PRODUCT, p)));

            if (!filteredProducts.Any())
            {
                sb.AppendLine(OutputConstants.EMPTY_BASKET);
                return sb.ToString().TrimEnd();
            }

            Queue<string> products = new Queue<string>();
            filteredProducts.ForEach(x => products.Enqueue(x.ToLower()));

            double totalCost = 0.00;
            bool tft = model.TwoForThree;
            bool hp = model.HalfPrice;

            if (model.TwoForThree)
            {
                var dealProducts = tftDeal.Arguments.Split(", ");

                foreach (var p in dealProducts)
                {
                    if (!products.Contains(p))
                    {
                        tft = false;
                    }
                }
            }

            if (tft)
            {
                string freeProduct = products.Skip(2).Take(1).First();
                totalCost += GetTwoForThreePrice(products, dbProducts);

                sb.AppendLine(string.Format(OutputConstants.TWO_FOR_THREE_DEAL, freeProduct, tftDeal.Type));
            }

            if (hp && products.Contains(hpDeal.Arguments))
            {
                totalCost += GetTotalPriceWithDiscount(products, dbProducts, hpDeal.Arguments);
                sb.AppendLine(string.Format(OutputConstants.HALF_PRICE_DEAL, hpDeal.Arguments, hpDeal.Type));
            }
            else
            {
                totalCost += GetTotalPriceWithoutDiscount(products, dbProducts);
            }

            var aws = totalCost / 100;
            sb.AppendLine(string.Format(OutputConstants.TOTAL_COST, aws, totalCost));

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Processed the first three products and removes them from the order.
        /// The Cheapest one is free.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="dbProducts"></param>
        /// <returns>The total cost for the first three products</returns>
        private double GetTwoForThreePrice(Queue<string> products, IEnumerable<ProductViewModel> dbProducts)
        {
            var dealPr = products.Take(3).ToList();

            for (int i = 0; i < 3; i++)
            {
                products.Dequeue();
            }

            var prices = new List<double>();

            dealPr.ForEach(p
                => prices.Add(dbProducts.Where(pr => pr.Name == p).First().Price));

            double cost = prices.OrderByDescending(p => p).Take(2).Sum();

            return cost;
        }

        /// <summary>
        /// Gets the total price for the order with the 'buy 1 get 1 for half price' discount included.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="dbProducts"></param>
        /// <param name="discounted"></param>
        /// <returns>Total Price</returns>
        private double GetTotalPriceWithDiscount(Queue<string> products, IEnumerable<ProductViewModel> dbProducts, string discounted)
        {
            double totalCost = 0;

            List<string> productsList = products.ToList();

            while (productsList.Any())
            {
                string curr = productsList.First();

                if (curr == discounted)
                {
                    if (productsList.Where(p => p == curr).Count() >= 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            productsList.Remove(curr);
                        }

                        double price = dbProducts.Where(p => p.Name == curr).First().Price;
                        totalCost += price + (price / 2);
                        continue;
                    }

                    totalCost += dbProducts.Where(p => p.Name == curr).First().Price;
                    productsList.Remove(curr);
                }
                else
                {
                    totalCost += dbProducts.Where(p => p.Name == curr).First().Price;
                    productsList.Remove(curr);
                }
            }

            return totalCost;
        }

        /// <summary>
        /// Gets the total price for the order without any discounts.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="dbProducts"></param>
        /// <returns>Total Price</returns>
        private double GetTotalPriceWithoutDiscount(Queue<string> products, IEnumerable<ProductViewModel> dbProducts)
        {
            double totalCost = 0;

            while(products.Any())
            {
                string currProduct = products.Dequeue();
                totalCost += dbProducts.Where(p => p.Name == currProduct).First().Price;
            }

            return totalCost;
        }
    }
}
