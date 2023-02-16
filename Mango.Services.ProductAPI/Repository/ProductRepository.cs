using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductReposiory
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper1;

        public ProductRepository(ApplicationDbContext applicationDbContext, IMapper mapper1)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper1 = mapper1;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product = mapper1.Map<Product>(productDto);
            if (product.ProductId > 0)
            {
                applicationDbContext.Products.Update(product);
            }
            else
            {
                applicationDbContext.Products.Add(product);
            }
            await applicationDbContext.SaveChangesAsync();
            return mapper1.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                Product product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if(product == null)
                {
                    return false;
                }
                applicationDbContext.Products.Remove(product);
                await applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProduct()
        {
            List<Product> products = await applicationDbContext.Products.ToListAsync();
            return mapper1.Map<List<Models.DTO.ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            Product product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            return mapper1.Map<Product, ProductDto>(product);
        }
    }
}
