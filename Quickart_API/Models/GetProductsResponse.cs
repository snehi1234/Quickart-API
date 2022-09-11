using System;
namespace Quickart_API.Models
{
    public class Product
    {
        public int product_id { get; set; }
        public string? product_name { get; set; }
        public int product_price { get; set; }
        public string? product_short_description { get; set; }
        public string? product_long_description { get; set; }
        public string? product_image_url { get; set; }
        public string? product_barcode { get; set; }
    }

    public class GetProductsResponse
    {
        public int response_code { get; set; }
        public List<Product>? products { get; set; }
        public Exception? exception { get; set; }

    }
}

