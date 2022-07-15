using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegisterDirectory.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegisterDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> CreateUser()
        {
            await _roleManager.CreateAsync(new() { Name = "Admin" });
            await _roleManager.CreateAsync(new() { Name = "Editor" });
            var user= new AppUser() { UserName = "kerem", City = "İstanbul" };
            await _userManager.CreateAsync(user,"Password123*");
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok();
        }
        public async Task<IActionResult> GeToken(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("username ya da pass yanlış");
            }
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest("username ya da pass yanlış");
            }
            var accessTokenExpiration = DateTime.Now.AddMinutes(60);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfghjmköçasfgasfgasfasjfasgasga996689***"));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: "www.myapi.com",
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: new List<Claim>() { new Claim("city",user.City)},
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }
    }
   
}
