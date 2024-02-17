using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Users
{
    public class BaseUserProfileUpdateRequest
    {

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
    }
}
