using GroceryShopWeb.Core.Contracts;
using GroceryShopWeb.Core.ViewModels.Product;
using GroceryShopWeb.Infrastructure.DataConstants;
using Microsoft.AspNetCore.Mvc;

namespace GroceryShopWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService productService;
        private readonly IDealService dealService;
        private readonly IOrderService orderService;

        public ShopController(
            IProductService _productService, 
            IDealService _dealService,
            IOrderService _orderService)
        {
            this.productService = _productService;
            this.dealService = _dealService;
            this.orderService = _orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var model = await this.productService.GetAllProductsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Products([FromForm]int id, [FromForm]int price)
        {
            await this.productService.UpdatePriceAsync(id, price);

            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public async Task<IActionResult> Deals()
        {
            ViewData["Allowed"] = await this.productService.GetAllProductNamesAsync();

            var model = await dealService.GetAllDealsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deals([FromForm]int id,[FromForm]string arguments)
        {
            await this.dealService.UpdateDealAsync(id, arguments.Trim());

            return RedirectToAction(nameof(Deals));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new OrderViewModel()
            {
                Products = await productService.GetAllProductsAsync()
            };

            ViewData["Allowed"] = await this.productService.GetAllProductNamesAsync();
            ViewData[DealConstants.TwoForThree] = (await dealService.GetTwoForThreeDealAsync()).Arguments;
            ViewData[DealConstants.HalfPrice] = (await dealService.GetHalfPriceDealAsync()).Arguments;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderViewModel model)
        {
            var result = (await orderService.ProcessOrderAsync(model)).Split("\n");

            return View("CheckoutResult", result);
        }

        [HttpPost]
        [Route("/Shop/AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm]string productName, [FromForm]int productPrice)
        {
            var model = new AddProductViewModel()
            {
                Name = productName,
                Price = productPrice
            };

            if (!TryValidateModel(model))
            {
                return RedirectToAction(nameof(Products));
            }

            await this.productService.AddProductAsync(model);

            return RedirectToAction(nameof(Products));
        }
    }
}
