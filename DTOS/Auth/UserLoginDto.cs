using System.ComponentModel.DataAnnotations;

namespace ChatApp_Api.DTOS.Auth
{
    public class UserLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
