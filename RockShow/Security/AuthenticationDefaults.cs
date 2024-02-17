using Microsoft.AspNetCore.Authentication.Cookies;

namespace RockShow.Security
{
    internal static class AuthenticationDefaults
    {
        public const string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }
}