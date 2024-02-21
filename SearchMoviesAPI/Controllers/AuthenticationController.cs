using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SearchMoviesAPI.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace SearchMoviesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOptions<List<LoginModel>> _login;

        public AuthenticationController(IOptions<List<LoginModel>> login,
                                      IConfiguration configuration,
                                      ILogger<AuthenticationController> logger)
        {
            _login = login;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                _logger.LogInformation($"validating user credentials.. {JsonConvert.SerializeObject(model)}");
                var user = _login.Value.Find(c => c.UserName == model.UserName && c.Password == model.Password);
                if (user != null)
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "User")
                };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return await Task.FromResult(Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    }));
                }

                return Unauthorized(new LoginResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Response = $"User not found or Enter credentials correctly"
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
