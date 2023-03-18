using System.ComponentModel.DataAnnotations;

namespace ChatApp_Api.Models
{
    public class Friend
    {
        public int Id { get; set; }
        [Required]
        public string User1Id { get; set; }
        [Required]

        public string User2Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}
