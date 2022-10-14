using System;
namespace Quickart_API.Models
{
    public class Products
    {
        public string product_id { get; set; }
        public int product_qty_cnt { get; set; }
    }

    public class OrderDetails
    {
        public string store_id { get; set; }
        public List<Products> products { get; set; }
    }

    public class PlaceOrderRequest
	{
        public string token { get; set; }
        public string paymentType { get; set; }
        public string paymentReference { get; set; }
        public string date { get; set; }
        public string purchaseType { get; set; }
        public string address { get; set; }
        public string cellNumber { get; set; }
        public List<OrderDetails> orderDetails { get; set; }
    }
}

