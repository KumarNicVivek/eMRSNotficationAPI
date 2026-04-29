namespace AuthService.Models
{
    public class LoginResponseModel
    {
        public string token { get; set; }
        public string Role { get; set; }
        public List<string> Permissions { get; set; }
    }
}
