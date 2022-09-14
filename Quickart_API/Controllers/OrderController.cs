using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quickart_API.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quickart_API.Controllers
{
    [Route("Order")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("PlaceOrder", Name = nameof(PlaceOrderAsync))]
        public async Task<ActionResult<PlaceOrderResponse>> PlaceOrderAsync([FromBody] PlaceOrderRequest request)
        {
            try
            {


                var response = new PlaceOrderResponse
                {
                    response_code = 404,
                    response_message = ""
                };

                return response;
            }
            catch (Exception e)
            {
                var response = new PlaceOrderResponse
                {
                    response_code = 404,
                    response_message = e.ToString()
                };

                return response;
            }

        }
    }
}

