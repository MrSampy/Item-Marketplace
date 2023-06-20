using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Business.Models;
namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class AuthController
    {
        private readonly IAuthService authService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            this.authService = authService;
        }
        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LoginModel logInModel)
        {
            UserModel result;
            try
            {
                result = await authService.GetUserByCredentials(logInModel.NickName, logInModel.Password);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            var tokenString = GenerateJwtToken($"{result.Name}_{result.Surname}");
            return new OkObjectResult(new { token = tokenString });
        }
    }
}
