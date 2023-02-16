using System.ComponentModel.DataAnnotations;

namespace Restrau.Services.OrderAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartheaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set;}
    }
}
