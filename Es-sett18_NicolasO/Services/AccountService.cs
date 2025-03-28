using Es_sett18_NicolasO.DTOs;
using Es_sett18_NicolasO.Models;
using Microsoft.AspNetCore.Identity;

namespace Es_sett18_NicolasO.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> Register(RegisterDto registerDto, string role)
        {
            var user = new ApplicationUser { UserName = registerDto.Email, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            }

            await _userManager.AddToRoleAsync(user, role);

            return user;
        }

        public async Task<ApplicationUser> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception("Utente non trovato");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("Password non valida");
            }

            return user;
        }
    }
}
