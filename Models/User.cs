using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApp_Api.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

    }
}
