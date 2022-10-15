using System;
namespace Quickart_API.Models
{
    public class DeliveryPerson
    {
        public int user_id { get; set; }
        public string? name { get; set; }
    }

    public class Order
    {
        public int order_id { get; set; }
        //public int order_status_id { get; set; }
        public string? order_by { get; set; }
    }

    public class OrdersResponse
	{
		public int response_code { get; set; }
		public List<DeliveryPerson>? delivery_people { get; set; }
		public List<Order>? orders { get; set; }
        public string? response_message { get; set; }
	}
}

