using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Email
{
    public class BaseEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
