using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory clientFactory;

        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        public async Task<T> AddCartAsync<T>(CartDto cartDto, string token = null)
        {     
            return await this.SendAsync<T>(new ApiRequest()
            { 
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                ApiUrl = SD.CartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }     
              
        public async Task<T> GetCartAsync<T>(string id, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.CartAPIBase + "/api/cart/GetCart/"+id,
                AccessToken = token
            });
        }

        public async Task<T> RemoveCartAsync<T>(int cartId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartId,
                ApiUrl = SD.CartAPIBase + "/api/cart/RemoveCart" ,
                AccessToken = token
            });
        }
        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                ApiUrl = SD.CartAPIBase + "/api/cart/updateCart",
                AccessToken = token
            });
        }
    }
}
