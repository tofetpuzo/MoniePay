using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CreateDepositRequest
    {
        [JsonPropertyName("First Name")]
        [Required(ErrorMessage = "first name is required")]
        public required string FirstName { get; set; }

        [JsonPropertyName("Last Name")]
        [Required(ErrorMessage = "last name is required")]
        public required string LastName { get; set; }

        [JsonPropertyName("Currency")]
        [Required(ErrorMessage = "Currency is required")]
        public required Currency currency { get; set; }

        [JsonPropertyName("Amount")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public Channel channel { get; set; } = Channel.API;

        [JsonPropertyName("idempotencyKey")]
        [Required(ErrorMessage = "idempotencyKey is required")]
        public required string IdempotencyKey { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("confirm")]
        public bool Confirm { get; set; } = true;

        [JsonPropertyName("DestinationAccountNumber")]
        [Required(ErrorMessage = "DestinationAccountNumber is required")]
        public required string DestinationAccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("DestinationAccountName")]
        [Required(ErrorMessage = "DestinationAccountName is required")]
        public required string DestinationAccountName { get; set; } = string.Empty;
    }
}
