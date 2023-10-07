using DrivingSchoolPlatform.Api.Data;
using DrivingSchoolPlatform.Shared.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrivingSchoolPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<IdentityUser> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager,
            ILogger<IdentityUser> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("/account/getme"), Authorize]
        public async Task<IActionResult> GetMe()
        {
            IdentityUser? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user != null)
            {
                return new JsonResult(user.Id);
            }
            else
                return NotFound("User not found");
        }

        [HttpPost("/account/register"), Authorize(Roles = "Admin")]
        //[HttpPost("/account/register")]
        public async Task<IActionResult> Register([FromBody] AccountRegistration accountRegistration)
        {
            IdentityUser user = new IdentityUser { PhoneNumber = accountRegistration.PhoneNumber, UserName = accountRegistration.PhoneNumber };

            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, accountRegistration.Password);

            if (!result.Succeeded)
                return BadRequest(String.Join(Environment.NewLine, result.Errors));

            await _userManager.AddToRoleAsync(user, accountRegistration.Role);

            return Ok();
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] AccountLogin accountLogin)
        {
            var token = "";

            var user = await _userManager.FindByNameAsync(accountLogin.PhoneNumber);
            var valid = await _signInManager.UserManager.CheckPasswordAsync(user, accountLogin.Password);

            if (valid)
            {
                token = await CreateToken(user);
                _logger.LogInformation("User logged in.");

                // проверяем, принадлежит ли URL приложению
                if (!string.IsNullOrEmpty(accountLogin.ReturnUrl) && Url.IsLocalUrl(accountLogin.ReturnUrl))
                {
                    accountLogin.ReturnUrl = accountLogin.ReturnUrl;
                }
                else
                {
                    accountLogin.ReturnUrl = "/";
                }
            }
            else
            {
                _logger.LogWarning("Invalid login and (or) password");
                return Unauthorized("Неправильный логин и (или) пароль");
            }

            return Ok(new AccountLoginResult(token, accountLogin.ReturnUrl));
        }

        private async Task<string> CreateToken(IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("/roles/create"), Authorize(Roles = "Admin")]
        //[HttpPost("/roles/create")]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok(name);
        }
    }
}
