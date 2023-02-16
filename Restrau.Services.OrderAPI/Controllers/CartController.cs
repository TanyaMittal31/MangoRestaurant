using Microsoft.AspNetCore.Mvc;
using Restrau.Services.OrderAPI.Models.Dto;
using Restrau.Services.OrderAPI.Repository;

namespace Restrau.Services.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        protected ResponseDto responseDto;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
            this.responseDto = new ResponseDto();
        }
        /*public IActionResult Index()
        {
            return View();
        }*/

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await cartRepository.GetCartByUserId(userId);
                responseDto.Result = cartDto;
            }catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await cartRepository.CreateUpdateCart(cartDto);
                responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await cartRepository.CreateUpdateCart(cartDto);
                responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                bool result = await cartRepository.RemoveFromCart(cartDetailsId);
                responseDto.Result = result;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return responseDto;
        }
    }
}
