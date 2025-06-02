using dotnetExam2.DTOs;
using dotnetExam2.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnetExam2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository TokenRepository)
        {
            this.userManager = userManager;
            tokenRepository=TokenRepository;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered. Please login");
                    }
                }
            }

            return BadRequest("Something went wrong while registering user");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username); 

            if (user != null)
            {
                var checkedPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkedPasswordResult)
                {
                    // get roles for user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResposeDto
                        {
                            jwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                    
                }
            }
            return BadRequest("Username or password is incorrect");
        }
    }
}
