using Microsoft.AspNetCore.Session;

namespace Server_cloudata.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SessionId { get; set; }
    }
}
