using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Email
{
    public class ContactUsRequest : BaseEmailRequest
    {
        [Required]
        public string From { get; set; }
    }
}
