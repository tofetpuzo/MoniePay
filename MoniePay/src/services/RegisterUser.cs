namespace MoniePay.src.services
{
    public class RegisterUser
    {
        public required string Username;
        public required string Password;
        public required string Email;
        public RegisterUser(UserDetails userDetails)
        {
            Username = userDetails.username;
            Password = userDetails.password;
            Email = userDetails.email;
        }
    }
    public sealed record UserDetails
    (
        string username,
        string password,
        string email
    );
}
