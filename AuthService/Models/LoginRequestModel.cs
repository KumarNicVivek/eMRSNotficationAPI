namespace AuthService.Models
{
    public class LoginRequestModel
    {
        public string username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //public string userEcryptCode { get; set; } = string.Empty;
        //public string? passwordSalt { get; set; }
    }
}
