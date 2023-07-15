using Microsoft.AspNetCore.Session;

namespace Server_cloudata.DTO
{
    public class LoginDTO : BaseBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
