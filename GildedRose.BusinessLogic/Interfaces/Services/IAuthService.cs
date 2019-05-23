using System.Security.Claims;
using GildedRose.BusinessLogic.DTOs;

namespace GildedRose.BusinessLogic.Interfaces.Services
{
    public interface IAuthService
    {
        string GetUserToken(string username, string password);
        UserDto GetUserClaims(ClaimsIdentity identityClaims);
    }
}
