using Es_sett18_NicolasO.DTOs;
using Es_sett18_NicolasO.Services;
using Microsoft.AspNetCore.Mvc;

namespace Es_sett18_NicolasO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                var user = await _accountService.Register(registerDto, "Utente");
                var token = await _tokenService.CreateToken(user);

                return Ok(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _accountService.Login(loginDto);
                var token = await _tokenService.CreateToken(user);

                return Ok(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
