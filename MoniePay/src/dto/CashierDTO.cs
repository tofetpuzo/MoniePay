using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CashierDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;
    }
}
