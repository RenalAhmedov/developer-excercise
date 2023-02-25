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
    [TestFixture]
    public class ProductServiceTests : UnitTestsBase
    {
        private IProductService productService;

        [OneTimeSetUp]
        public async Task Setup()
        {
            productService = new ProductService(this.context);
        }

        [Test]
        public async Task AddProductAsync_CreatesNewProduct()
        {
            int countBeforeAdd = this.context.Products.Count();

            await productService.AddProductAsync(new AddProductViewModel()
            {
                Name = "test",
                Price = 40
            });

            int countAfterAdd = this.context.Products.Count();

            Assert.That(countAfterAdd, Is.EqualTo(countBeforeAdd + 1));
        }

        [Test]
        public async Task AddProductAsync_DoesntAddIfProductAlreadyExists()
        {
            var product = await this.context.Products.FirstAsync();
            int countBeforeAdd = this.context.Products.Count();

            await productService.AddProductAsync(new AddProductViewModel()
            {
                Name = product.Name,
                Price = 40
            });

            int countAfterAdd = this.context.Products.Count();

            Assert.That(countBeforeAdd, Is.EqualTo(countAfterAdd));
        }

        [Test]
        public async Task GetAllProductNamesAsync_ReturnsCorrectString()
        {
            var expected = string.Join(", ", context.Products.Select(p => p.Name));

            var actual = await productService.GetAllProductNamesAsync();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsCorrect()
        {
            var expectedCount = this.context.Products.Count();
            var actualCount = (await productService.GetAllProductsAsync()).Count();

            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task UpdatePriceAsync_UpdatesPrice()
        {
            var product = this.context.Products.Find(1);

            var priceBeforeUpdate = product.Price;

            await productService.UpdatePriceAsync(product.Id, 20);

            Assert.That(product.Price, Is.Not.EqualTo(priceBeforeUpdate));
        }

        [Test]
        public async Task UpdatePriceAsync_DoestUpdateWithInvalidPrice()
        {
            var product = this.context.Products.Find(1);

            var priceBeforeUpdate = product.Price;

            await productService.UpdatePriceAsync(product.Id, 0);
            await productService.UpdatePriceAsync(product.Id, -20);
            await productService.UpdatePriceAsync(product.Id, 12345);

            Assert.That(product.Price, Is.EqualTo(priceBeforeUpdate));
        }
    }
}
