using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Silicon_WebApi.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class TokenController(IConfiguration config) : ControllerBase
{
    private readonly IConfiguration _config = config;

    [HttpPost]
    public IActionResult GetToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return Ok(tokenString);
    }
}
