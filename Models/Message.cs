namespace ChatApp_Api.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? MessageBody { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
