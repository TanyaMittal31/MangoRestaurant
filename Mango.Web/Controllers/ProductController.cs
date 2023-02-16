using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ActionName("ProductView")]
        public async Task<IActionResult> ProductView()
        {
            List<ProductDto> products = new List<ProductDto>();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetAllProductsAsync<ResponseDto>(accessToken);
            if(response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.CreateProductAsync<ResponseDto>(product,accessToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductView));
                }                
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int ProductId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<ResponseDto>(ProductId, accessToken);
            if (response != null && response.IsSuccess)
            {
                ProductDto products = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(products);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.UpdateProductAsync<ResponseDto>(product, accessToken);
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductView));
                }
            }
            return View(product);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(int ProductId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<ResponseDto>(ProductId, accessToken);
            if (response != null && response.IsSuccess)
            {
                ProductDto products = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(products);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.DeleteProductAsync<ResponseDto>(product.ProductId, accessToken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductView));
                }
            }
            return View(product);
        }
    }
}
