using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwtDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private const string SECRET_KEY = "abcdef159753123  abcd";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        [HttpGet]
        public IActionResult Get([FromHeader]string username,[FromHeader]string password)
        {
            if(username ==password)
            {
                return new ObjectResult(GenerateToken(username));
            }else
            {
                return BadRequest();
            }
        }

        private string GenerateToken(string username)
        {
            var token = new JwtSecurityToken(
                claims:new Claim[]
                {
                    new Claim(ClaimTypes.Name,username)
                },
                notBefore:new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials:new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
