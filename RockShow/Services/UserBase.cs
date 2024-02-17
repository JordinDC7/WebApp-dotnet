using RockShow.Interfaces;

namespace RockShow.Services
{
    public class UserBase : IUserAuthData
    {
        public int Id
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string Role
        {
            get; set;
        }

        public object TenantId
        {
            get; set;
        }

        public string AvatarUrl
        {
            get; set;
        }
    }
}
