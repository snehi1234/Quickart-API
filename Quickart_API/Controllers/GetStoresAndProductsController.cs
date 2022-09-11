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

namespace Quickart_API.Controllers
{
    [Route("GetStoresAndProducts")]
    [ApiController]
    public class GetStoresAndProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public GetStoresAndProductsController(IConfiguration configuration)
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
                string Email = jwtToken.Claims.First(x => x.Type == "id").ToString();
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return Email;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        [HttpGet("GetStores", Name = nameof(GetStoresAsync))]
        public async Task<ActionResult<GetStoresResponse>> GetStoresAsync([FromBody] GetStoresRequest request)
        {
            try
            {
                string validate_token = validate(request.token);
                bool validUser = true;
                if (validate_token == null)
                {
                    validUser = false;
                    var response = new GetStoresResponse
                    {
                        response_code = 500
                    };

                    return response;
                }
                else
                {
                    string st = ("SELECT * FROM Stores");
                    DataTable table = new DataTable();
                    List<Store> StoreList = new List<Store>();
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

                        if (table.Rows.Count > 0)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                Store s = new Store();
                                s.store_address = row["store_address"].ToString();
                                s.store_contact_number = row["store_contact_number"].ToString();
                                s.store_email = row["store_email"].ToString();
                                s.store_id = Convert.ToInt32(row["store_id"]);
                                s.store_lat = row["store_lat"].ToString();
                                s.store_long = row["store_long"].ToString();
                                s.store_name = row["store_name"].ToString();

                                StoreList.Add(s);
                            }
                        }
                    }

                    var response = new GetStoresResponse
                    {
                        response_code = 200,
                        stores = StoreList
                    };

                    return response;
                }


            }
            catch (Exception e)
            {
                var response = new GetStoresResponse
                {
                    exception = e
                };
                return response;
            }
        }



        [HttpGet("GetProducts", Name = nameof(GetProductsAsync))]
        public async Task<ActionResult<GetProductsResponse>> GetProductsAsync([FromBody] GetProductsRequest request)
        {
            try
            {

                string validate_token = validate(request.token);
                bool validUser = true;
                if (validate_token == null)
                {
                    validUser = false;
                    var response = new GetProductsResponse
                    {
                        response_code = 500
                    };

                    return response;
                }
                else
                {
                    int StoreID = request.StoreID;
                    List<Product> ProductList = new List<Product>();
                    string st = ("select * from Products where product_id in (select distinct(Product_id) from store_product where store_id = " + StoreID + ")");
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

                        if (table.Rows.Count > 0)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                Product p = new Product();
                                p.product_id = Convert.ToInt32(row["product_id"].ToString());
                                p.product_image_url = row["product_image_url"].ToString();
                                p.product_long_description = row["product_long_description"].ToString();
                                p.product_name = row["product_name"].ToString();
                                p.product_price = Convert.ToInt32(row["product_price"].ToString());
                                p.product_short_description = row["product_short_description"].ToString();
                                p.product_barcode = row["product_barcode"].ToString();

                                ProductList.Add(p);
                            }
                        }
                    }

                    var response = new GetProductsResponse
                    {
                        response_code = 200,
                        products = ProductList
                    };

                    return response;
                }
            }
            catch (Exception e)
            {
                var response = new GetProductsResponse
                {
                    exception = e
                };

                return response;

            }
        }

    }
}

