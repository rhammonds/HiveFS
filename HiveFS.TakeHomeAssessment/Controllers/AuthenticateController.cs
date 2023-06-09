using HiveFS.TakeHomeAssessment.Controllers;
using HiveFS.TakeHomeAssessment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthenticationandAuthorization.Controllers
{
    public class AuthenticateController : HiveFsBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthenticateController(IConfiguration configuration, ILogger<FruitController> logger )
        {
            _configuration = configuration;
            _logger = logger;
        }
       
        private string GenerateJSONWebToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
              
        private async Task<LoginModel> AuthenticateUser(LoginModel login)
        {
            if (login.UserId == _configuration["userid"] && login.Password == _configuration["password"])
            {
                return login;
            }
            return null;
        }
               
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(loginModel);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            _logger.LogError($"Login failed for userId: {loginModel.UserId}");
            return response;
        }
       
        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }
    }
}