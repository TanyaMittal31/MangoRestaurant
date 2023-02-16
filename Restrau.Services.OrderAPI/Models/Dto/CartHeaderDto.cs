using System.ComponentModel.DataAnnotations;

namespace Restrau.Services.OrderAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartheaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set;}
    }
}
