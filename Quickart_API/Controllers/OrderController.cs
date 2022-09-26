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

                return Email;
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
    }
}

