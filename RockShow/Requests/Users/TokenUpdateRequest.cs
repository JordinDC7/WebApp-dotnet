using System.ComponentModel.DataAnnotations;

namespace RockShow.Requests.Users
{
    public class TokenUpdateRequest
    {
        public string TokenId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TokenType { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
