using MoniePay.src.auth;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class UserResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("Role")]
        public Roles.RoleType UserRole { get; set; }

        [JsonPropertyName("isUserActive")]
        public bool Active { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        /// <summary>Maps entity -> wire. The only place this translation happens.</summary>
        public static UserResponse From(CreateUserRequest createUserRequest) => new()
        {
            Id = createUserRequest.Id,
            Username = createUserRequest.Username,
            UserRole = createUserRequest.RoleFlags,
            Active = createUserRequest.isUserActive
        };
    }
}
