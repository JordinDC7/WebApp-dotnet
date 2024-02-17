using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Users
{
    public class TokenAddRequest
    {
        [Required]
        public string TokenId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TokenType { get; set; }
    }
}
