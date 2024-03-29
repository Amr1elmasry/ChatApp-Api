﻿namespace ChatApp_Api.DTOS.Auth
{
    public class UserReturnDto
    {
        public string? Id { get; set; }
        public string? Massage { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool IsAuth { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}
