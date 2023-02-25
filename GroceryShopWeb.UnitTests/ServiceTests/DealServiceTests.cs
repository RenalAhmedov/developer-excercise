using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.Services;
using GroceryShopWeb.Infrastructure.DataConstants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShopWeb.UnitTests.ServiceTests
{
    [TestFixture]
    public class DealServiceTests : UnitTestsBase
    {
        private IDealService dealService;
        private IProductService productService;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this.productService = new ProductService(this.context);
            this.dealService = new DealService(this.context, this.productService);
        }

        [Test]
        public async Task GetAllDealsAsync_ReturnsCorrect()
        {
            int expected = this.context.Deals.Count();
            int actual = (await this.dealService.GetAllDealsAsync()).Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetHalfPriceDealAsync_ReturnsCorrect()
        {
            var expected = await this.context.Deals.Where(d => d.Type == DealConstants.HalfPrice)
                .FirstOrDefaultAsync();

            var actual = await this.dealService.GetHalfPriceDealAsync();

            Assert.That(expected.Id, Is.EqualTo(actual.Id));
            Assert.That(expected.Type, Is.EqualTo(actual.Type));
            Assert.That(expected.Arguments, Is.EqualTo(actual.Arguments));
        }

        [Test]
        public async Task GetTwoForThreeDealAsync_ReturnsCorrect()
        {
            var expected = await this.context.Deals.Where(d => d.Type == DealConstants.TwoForThree)
                .FirstOrDefaultAsync();

            var actual = await this.dealService.GetTwoForThreeDealAsync();

            Assert.That(expected.Id, Is.EqualTo(actual.Id));
            Assert.That(expected.Type, Is.EqualTo(actual.Type));
            Assert.That(expected.Arguments, Is.EqualTo(actual.Arguments));
        }

        [Test]
        public async Task UpdateDealAsync_UpdatesHalfPriceDealCorrect()
        {
            var deal = await this.context.Deals.Where(d => d.Type == DealConstants.HalfPrice)
                .FirstAsync();

            string argsBeforeUpdate = deal.Arguments;
            string newArgs = "apple";

            await this.dealService.UpdateDealAsync(deal.Id, newArgs);

            Assert.That(argsBeforeUpdate, Is.Not.EqualTo(deal.Arguments));
            Assert.That(newArgs, Is.EqualTo(deal.Arguments));
        }

        [Test]
        public async Task UpdateDealAsync_DoesntUpdateHalfPriceIfProductDoesntExist()
        {
            var deal = await this.context.Deals.Where(d => d.Type == DealConstants.HalfPrice)
                .FirstAsync();

            string argsBeforeUpdate = deal.Arguments;
            string argsProduct = "nonexistent";

            await this.dealService.UpdateDealAsync(deal.Id, argsProduct);

            Assert.That(argsBeforeUpdate, Is.EqualTo(deal.Arguments));
            Assert.That(!this.context.Products.Any(p => p.Name == argsProduct));
        }

        [Test]
        public async Task UpdateDealAsync_UpdatesTwoForThreeDealCorrect()
        {
            var deal = await this.context.Deals.Where(d => d.Type == DealConstants.TwoForThree)
                .FirstAsync();

            string argsBeforeUpdate = deal.Arguments;
            string newArgs = "apple, banana, potato";

            await this.dealService.UpdateDealAsync(deal.Id, newArgs);

            Assert.That(argsBeforeUpdate, Is.Not.EqualTo(deal.Arguments));
            Assert.That(newArgs, Is.EqualTo(deal.Arguments));
        }

        [Test]
        public async Task UpdateDealAsync_DoesntUpdateTwoForThreeDealIfArgumentsAreNotExactly_Three_And_Distinct()
        {
            var deal = await this.context.Deals.Where(d => d.Type == DealConstants.TwoForThree)
                .FirstAsync();

            string argsBeforeUpdate = deal.Arguments;
            string newArgsFour = "apple, banana, potato, tomato";
            string newArgsTwo = "apple, tomato";
            string notDistinct = "apple, apple, tomato";

            await this.dealService.UpdateDealAsync(deal.Id, newArgsFour);
            await this.dealService.UpdateDealAsync(deal.Id, newArgsTwo);
            await this.dealService.UpdateDealAsync(deal.Id, notDistinct);

            Assert.That(argsBeforeUpdate, Is.EqualTo(deal.Arguments));
        }

        [Test]
        public async Task UpdateDealAsync_DoesntUpdateTwoForThreeDealIfProductDoesntExist()
        {
            var deal = await this.context.Deals.Where(d => d.Type == DealConstants.TwoForThree)
                .FirstAsync();

            string argsBeforeUpdate = deal.Arguments;
            string argsProduct = "nonexistent";
            string newArgs = $"apple, banana, {argsProduct}";

            await this.dealService.UpdateDealAsync(deal.Id, newArgs);

            Assert.That(argsBeforeUpdate, Is.EqualTo(deal.Arguments));
            Assert.That(!this.context.Products.Any(p => p.Name == argsProduct));
        }
    }
}
