using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
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
        public string validate(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string Email = (jwtToken.Claims.First(x => x.Type == "email").Value).ToString();
                string UserID = (jwtToken.Claims.First(x => x.Type == "UserID").Value).ToString();

                return UserID;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        [HttpPost("PlaceOrder", Name = nameof(PlaceOrderAsync))]
        public async Task<ActionResult<PlaceOrderResponse>> PlaceOrderAsync([FromBody] PlaceOrderRequest request)
        {
            try
            {
                string validate_token = validate(request.token);
                bool validUser = true;
                if (validate_token == null)
                {
                    validUser = false;
                    var response = new PlaceOrderResponse
                    {
                        response_code = 500
                    };

                    return response;
                }
                else
                {
                    List<OrderDetails> order_list = new List<OrderDetails>();
                    order_list = request.orderDetails;

                    foreach (var order in order_list)
                    {
                        string storeId = order.store_id;
                        foreach (var product in order.products)
                        {
                            String prdId = product.product_id;
                            int prdQty = product.product_qty_cnt;
                            string st1 = "Insert into quickart_db.orders (order_placed_date, purchase_type, order_status_id, user_id, delivery_person, address_type, phone_number, store_id, payment_reference) values ('" + request.date + "','" + request.purchaseType + "'," + 1 + "," + validate_token+","+ 0 + ",'" + request.address + "','" + request.cellNumber + "','" + storeId + "','" + request.paymentReference + "')";
                            string st2 = "Insert into quickart_db.ordered_items (select max(o.order_id), sp.store_product_id,"+ prdQty +", p.product_price from orders o, store_product sp, products p where o.store_id = sp.store_id and sp.product_id ='" + prdId + "' and p.product_id ='" + prdId + "')";
                            DataTable table = new DataTable();
                            string DataSource = _configuration.GetConnectionString("QuickartCon");
                            MySqlDataReader myReader;

                            using (MySqlConnection mycon = new MySqlConnection(DataSource))
                            {
                                mycon.Open();
                                using (MySqlCommand mycommand = new MySqlCommand(st1, mycon))
                                {
                                    myReader = mycommand.ExecuteReader();
                                    table.Load(myReader);

                                    myReader.Close();
                                    mycon.Close();
                                }

                                mycon.Open();
                                using (MySqlCommand mycommand = new MySqlCommand(st2, mycon))
                                {
                                    myReader = mycommand.ExecuteReader();
                                    table.Load(myReader);

                                    myReader.Close();
                                    mycon.Close();
                                }
                            }

                        }
                    }

                    var response = new PlaceOrderResponse
                    {
                        response_code = 200,
                        response_message = ""
                    };

                    return response;

                }
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

        /*
        [HttpPost("OrderHistory", Name = nameof(OrderHistoryAsync))]
        public async Task<ActionResult<OrderHistoryResponse>> OrderHistoryAsync([FromBody] OrderHistoryRequest request)
        {

        }
        */

        [HttpPost("ChangeOrderStatus", Name = nameof(ChangeOrderStatusAsync))]
        public async Task<ActionResult<ChangeOrderStatusResponse>> ChangeOrderStatusAsync([FromBody] ChangeOrderStatusRequest request)
        {
            try
            {
                string validate_token = validate(request.token);
                bool validUser = true;
                if (validate_token == null)
                {
                    validUser = false;
                    var response = new ChangeOrderStatusResponse
                    {
                        response_code = 500
                    };

                    return response;
                }
                else
                {

                    string st = "update order set order_status_id = 4 where order_id = '" + request.order_id + "'";
                    DataTable table = new DataTable();
                    string DataSource = _configuration.GetConnectionString("QuickartCon");
                    MySqlDataReader myReader;

                    using (MySqlConnection mycon = new MySqlConnection(DataSource))
                    {
                        mycon.Open();
                        using (MySqlCommand mycommand = new MySqlCommand(st, mycon))
                        {
                            myReader = mycommand.ExecuteReader();
                            table.Load(myReader);

                            myReader.Close();
                            mycon.Close();
                        }
                    }
                        var response = new ChangeOrderStatusResponse
                    {
                        response_code = 200,
                        response_message = ""
                    };

                    return response;
                }

            }
            catch (Exception e)
            {
                var response = new ChangeOrderStatusResponse
                {
                    response_code = 404,
                    response_message = e.ToString()
                };

                return response;
            }



        }

    }
}

