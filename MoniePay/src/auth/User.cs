using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
namespace MoniePay.src.auth
{
    public class User : IdentityUser<Guid>
    {
        [JsonProperty("userId")]
        public Guid userId { get; set; } // you can map this to Id or remove duplicate

        [JsonProperty("username")]
        public string? username { get; set; }

        [JsonProperty("password")]
        public string? password { get; set; }
        [JsonProperty("isActive")]
        public bool? isActive { get; set; }

        [JsonProperty("isAdmin")]
        public bool? isAdmin { get; set; }

        [JsonProperty("email")]
        public string? email { get; set; } // IdentityUser already has Email

        [JsonProperty("roles")]
        public List<Roles> roles { get; set; } = new();

        [JsonProperty("roleFlags")]
        public Roles.RoleType RoleFlags { get; set; } = Roles.RoleType.None;

        [JsonProperty("createdOn")]
        public DateTime createdOn = DateTime.UtcNow;
    }
}
