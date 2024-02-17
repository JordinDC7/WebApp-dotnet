

using RockShow.Domain.LookUps;

namespace RockShow.Domain.Users
{
    public class User : BaseUser
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public LookUp Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public LookUp Role { get; set; }
    }
}
