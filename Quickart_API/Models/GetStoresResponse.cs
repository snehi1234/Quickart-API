﻿using System;
namespace Quickart_API.Models
{
    public class GetStoresResponse
    {
        public int response_code { get; set; }
        public List<Store>? stores { get; set; }
        public Exception exception { get; set; }
    }

    public class Store
    {
        public int store_id { get; set; }
        public string? store_name { get; set; }
        public string? store_address { get; set; }
        public string? store_contact_number { get; set; }
        public string? store_email { get; set; }
        public string? store_lat { get; set; }
        public string? store_long { get; set; }
    }
}

