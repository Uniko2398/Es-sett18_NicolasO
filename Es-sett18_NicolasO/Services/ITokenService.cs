using Es_sett18_NicolasO.Models;

namespace Es_sett18_NicolasO.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
