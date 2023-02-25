using GroceryShopWeb.Core.Constants;
using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.Services;
using GroceryShopWeb.Core.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShopWeb.UnitTests.ServiceTests
{
    public class OrderServiceTests : UnitTestsBase
    {
        private IDealService dealService;
        private IProductService productService;
        private IOrderService orderService;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this.productService = new ProductService(this.context);
            this.dealService = new DealService(context, productService);
            this.orderService = new OrderService(this.productService, this.dealService);
        }

        [Test]
        public async Task ProcessOrderAsync_ReturnsCorrectIfBasketIsEmpty()
        {
            string expected = OutputConstants.EMPTY_BASKET;

            string actual = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = ""
            });

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task ProcessOrderAsync_FiltersInvalidProducts()
        {
            string invalidProduct = "test123";
            string anotherInvalidProduct = "notInDatabase";
            var clouds = (await this.context.Products.Where(p => p.Name == "apple")
                .FirstAsync()).Price;
            var aws = clouds / 100;

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"apple, {invalidProduct}, {anotherInvalidProduct}"
            });

            Assert.That(result.Contains(string.Format(OutputConstants.INVALID_PRODUCT, invalidProduct)));
            Assert.That(result.Contains(string.Format(OutputConstants.INVALID_PRODUCT, anotherInvalidProduct)));
            Assert.That(result.Contains(string.Format(OutputConstants.TOTAL_COST, aws, clouds)));
        }

        [Test]
        public async Task ProcessOrderAsync_ConsidersBasketEmpty_IfOnlyInvalidProducts()
        {
            string invalidProduct = "test123";
            string anotherInvalidProduct = "notInDatabase";

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"{invalidProduct}, {anotherInvalidProduct}"
            });

            Assert.That(result.Contains(string.Format(OutputConstants.INVALID_PRODUCT, invalidProduct)));
            Assert.That(result.Contains(string.Format(OutputConstants.INVALID_PRODUCT, anotherInvalidProduct)));
            Assert.That(result.Contains(OutputConstants.EMPTY_BASKET));
        }

        //Used input from the task example at github
        [Test]
        public async Task ProcessOrderAsync_WorksCorrectWithBothDeals()
        {
            double aws = 1.99;
            double clouds = 199;
            string third = "banana";
            var tftDeal = await this.dealService.GetTwoForThreeDealAsync();
            var hpDeal = await this.dealService.GetHalfPriceDealAsync();
            string expectedPrice = string.Format(OutputConstants.TOTAL_COST, aws, clouds);
            string expectedTwoForThreeMessage = string.Format(OutputConstants.TWO_FOR_THREE_DEAL, third, tftDeal.Type);
            string expectedHalfPriceMessage = string.Format(OutputConstants.HALF_PRICE_DEAL, hpDeal.Arguments, hpDeal.Type);

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"apple, banana, {third}, potato, tomato, banana, potato",
                HalfPrice = true,
                TwoForThree = true
            });

            Assert.That(result.Contains(expectedPrice));
            Assert.That(result.Contains(expectedTwoForThreeMessage));
            Assert.That(result.Contains(expectedHalfPriceMessage));
        }

        //Used input from the task example at github
        [Test]
        public async Task ProcessOrderAsync_WorksCorrectWithoutDeals()
        {
            double aws = 2.52;
            double clouds = 252;
            string third = "banana";
            var tftDeal = await this.dealService.GetTwoForThreeDealAsync();
            var hpDeal = await this.dealService.GetHalfPriceDealAsync();
            string expectedPrice = string.Format(OutputConstants.TOTAL_COST, aws, clouds);
            string expectedTwoForThreeMessage = string.Format(OutputConstants.TWO_FOR_THREE_DEAL, third, tftDeal.Type);
            string expectedHalfPriceMessage = string.Format(OutputConstants.HALF_PRICE_DEAL, hpDeal.Arguments, hpDeal.Type);

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"apple, banana, {third}, potato, tomato, banana, potato",
                HalfPrice = false,
                TwoForThree = false
            });

            Assert.That(result.Contains(expectedPrice));
            Assert.That(!result.Contains(expectedTwoForThreeMessage));
            Assert.That(!result.Contains(expectedHalfPriceMessage));
        }

        //Used input from the task example at github
        [Test]
        public async Task ProcessOrderAsync_WorksCorrectWithOnly_TwoForThreeDeal()
        {
            double aws = 2.12;
            double clouds = 212;
            string third = "banana";
            var tftDeal = await this.dealService.GetTwoForThreeDealAsync();
            var hpDeal = await this.dealService.GetHalfPriceDealAsync();
            string expectedPrice = string.Format(OutputConstants.TOTAL_COST, aws, clouds);
            string expectedTwoForThreeMessage = string.Format(OutputConstants.TWO_FOR_THREE_DEAL, third, tftDeal.Type);
            string expectedHalfPriceMessage = string.Format(OutputConstants.HALF_PRICE_DEAL, hpDeal.Arguments, hpDeal.Type);

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"apple, banana, {third}, potato, tomato, banana, potato",
                HalfPrice = false,
                TwoForThree = true
            });

            Assert.That(result.Contains(expectedPrice));
            Assert.That(result.Contains(expectedTwoForThreeMessage));
            Assert.That(!result.Contains(expectedHalfPriceMessage));
        }

        //Used input from the task example at github
        [Test]
        public async Task ProcessOrderAsync_WorksCorrectWithOnly_HalfPriceDeal()
        {
            double aws = 2.39;
            double clouds = 239;
            string third = "banana";
            var tftDeal = await this.dealService.GetTwoForThreeDealAsync();
            var hpDeal = await this.dealService.GetHalfPriceDealAsync();
            string expectedPrice = string.Format(OutputConstants.TOTAL_COST, aws, clouds);
            string expectedTwoForThreeMessage = string.Format(OutputConstants.TWO_FOR_THREE_DEAL, third, tftDeal.Type);
            string expectedHalfPriceMessage = string.Format(OutputConstants.HALF_PRICE_DEAL, hpDeal.Arguments, hpDeal.Type);

            string result = await this.orderService.ProcessOrderAsync(new OrderViewModel()
            {
                Basket = $"apple, banana, {third}, potato, tomato, banana, potato",
                HalfPrice = true,
                TwoForThree = false
            });

            Assert.That(result.Contains(expectedPrice));
            Assert.That(!result.Contains(expectedTwoForThreeMessage));
            Assert.That(result.Contains(expectedHalfPriceMessage));
        }
    }
}
