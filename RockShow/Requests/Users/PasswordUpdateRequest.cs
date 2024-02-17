using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Users
{
    public class PasswordUpdateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 16)]
        public string Token { get; set; }

        [Required]
        [RegularExpression("^(?=.*\\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\\s).{8,100}$")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
