using System;
namespace Quickart_API.Models
{
    public class OrderDetails
    {
        public int store_id { get; set; }
        public List<Products>? products { get; set; }
    }

    public class Products
    {
        public int product_id { get; set; }
        public int product_qty_cnt { get; set; }
    }

    public class PlaceOrderRequest
	{
        public int user_id { get; set; }
        public string? payment_type { get; set; }
        public string? payment_reference { get; set; }
        public string? date { get; set; }
        public OrderDetails? order_details { get; set; }
     }
}

