using MoniePay.src.auth;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CreateAdminRequest
    {

        [JsonPropertyName("Username")]
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "password is required")]
        public required string password { get; set; }

        [JsonPropertyName("Email")]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [JsonProperty("roleFlags")]
        public Roles.RoleType RoleFlags { get; set; } = Roles.RoleType.Admin;

        [JsonProperty("createdOn")]
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
    }
}
