using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService: IBaseService
    {
        // T indicates that we will define the return type when we call it 
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDto productDto, string token);
        Task<T> UpdateProductAsync<T>(ProductDto productDto, string token);
        Task<T> DeleteProductAsync<T>(int id, string token);
    }
}
