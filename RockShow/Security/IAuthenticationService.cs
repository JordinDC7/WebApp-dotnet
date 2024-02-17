using System.Security.Claims;
using RockShow.Interfaces;

namespace RockShow.Security
{
    public interface IAuthenticationService<T> : IIdentityProvider<T>
    {
        Task LogInAsync(IUserAuthData user, params Claim[] extraClaims);

        Task LogOutAsync();

        bool IsLoggedIn();

        IUserAuthData GetCurrentUser();
    }
}
