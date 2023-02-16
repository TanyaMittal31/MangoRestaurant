namespace Mango.Web.Models
{
    public class CartHeaderDto
    {
        public int CartheaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
    }
}
