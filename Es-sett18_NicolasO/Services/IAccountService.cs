using Es_sett18_NicolasO.DTOs;
using Es_sett18_NicolasO.Models;

namespace Es_sett18_NicolasO.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser> Register(RegisterDto registerDto, string role);
        Task<ApplicationUser> Login(LoginDto loginDto);
    }
}
