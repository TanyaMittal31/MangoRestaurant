using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restrau.Services.OrderAPI.DbContexts;
using Restrau.Services.OrderAPI.Models;
using Restrau.Services.OrderAPI.Models.Dto;

namespace Restrau.Services.OrderAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper1;

        public CartRepository(ApplicationDbContext applicationDbContext, IMapper mapper1)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper1 = mapper1;
        }
        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await applicationDbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                applicationDbContext.CartDetails.RemoveRange(applicationDbContext.CartDetails.Where(x => x.CartHeaderId == cartHeaderFromDb.CartheaderId));
                applicationDbContext.CartHeaders.Remove(cartHeaderFromDb);
                await applicationDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = mapper1.Map<Cart>(cartDto);
            // check if prod exists in database, if not then we need to create it
            var productInDb = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
            if(productInDb == null)
            {
                applicationDbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await applicationDbContext.SaveChangesAsync();
            }

            //check if header is null
            var cartHeaderFromDb = await applicationDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.CartheaderId == cart.CartHeader.CartheaderId);

            if(cartHeaderFromDb== null)
            {
                // create header and details
                applicationDbContext.CartHeaders.Add(cart.CartHeader);
                await applicationDbContext.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartheaderId;
                cart.CartDetails.FirstOrDefault().Product = null;       // agr null nhi krenge to db product dubara add krne ki koshish krega
                applicationDbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());    
                await applicationDbContext.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                // check if details has same product
                var cartDetailsFromDb = await applicationDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == cart.CartDetails.FirstOrDefault().ProductId 
                && x.CartHeaderId == cartHeaderFromDb.CartheaderId );

                if( cartDetailsFromDb == null)
                {
                    //create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartheaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    applicationDbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    // update the count/ cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    applicationDbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault() );
                    await applicationDbContext.SaveChangesAsync();
                }
            }
            return mapper1.Map<CartDto>( cart );
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new() { 
                CartHeader = await applicationDbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId),
            };
            cart.CartDetails = applicationDbContext.CartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartheaderId).Include(x => x.Product);
            return mapper1.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            CartDetails cartDetails = await applicationDbContext.CartDetails.FirstOrDefaultAsync(x => x.CartDetailsId== cartDetailsId);
            int totalCountOfCartItems = applicationDbContext.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
            applicationDbContext.CartDetails.Remove(cartDetails);
            if(totalCountOfCartItems == 1)
            {
                var cartHeader = await applicationDbContext.CartHeaders.FirstOrDefaultAsync(x => x.CartheaderId == cartDetails.CartHeaderId);
                applicationDbContext.CartHeaders.Remove(cartHeader);

            }
            await applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
