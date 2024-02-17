using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Users
{
    public class UserAddRequest 
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

        [StringLength(2)]
        public string Mi { get; set; }

        [StringLength(255)]
        public string AvatarUrl { get; set; }

        [Required]
        [RegularExpression("^(?=.*\\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\\s).{8,100}$")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Range(1, 3)]
        public int RoleId { get; set; }
    }
}
