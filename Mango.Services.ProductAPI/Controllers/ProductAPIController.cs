using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductAPIController : Controller
    {
        private readonly IProductReposiory productReposiory;
        protected ResponseDto response;

        public ProductAPIController(IProductReposiory productReposiory)
        {
            this.productReposiory = productReposiory;
            this.response = new ResponseDto();
        }

        //[Authorize]
        [HttpGet]
        public async Task<object> GetAllProducts()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await productReposiory.GetProduct();
                response.Result = productDtos;
            }
            catch(Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        }

        [HttpGet]
        //[Authorize]
        [Route("{id:int}")]
        public async Task<object> GetProductById(int id)
        {
            try
            {
                ProductDto productDto = await productReposiory.GetProductById(id);
                response.Result = productDto;
            }
            catch(Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        }

        [HttpPost]
        [Authorize]
        public async Task<object> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto product = await productReposiory.CreateUpdateProduct(productDto);
                response.Result = product;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        }

        [HttpPut]
        [Authorize]
        public async Task<object> UpdateProduct([FromBody] ProductDto product)
        {
            try
            {
                ProductDto productDto = await productReposiory.CreateUpdateProduct(product);
                return response.Result = productDto;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<object> DeleteProduct(int id)
        {
            try
            {
                bool result = await productReposiory.DeleteProduct(id);
                response.Result = result;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        } 
    }
}
