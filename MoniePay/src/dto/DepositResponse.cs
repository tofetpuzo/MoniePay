using MoniePay.src.models;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class DepositResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        public static DepositResponse From(CreateDepositRequest depositRequest) => new()
        {
            //Id = intent.Id,
            //CustomerId = intent.CustomerId,
            //Amount = intent.Amount,
            //Currency = intent.Currency,
            //Status = intent.Status,
            //CreatedAt = intent.CreatedAt,
        };


        public static PaymentIntentResponse From(PaymentIntents intent) => new()
        {
            Id = intent.Id,
            CustomerId = intent.CustomerId,
            Amount = intent.Amount,
            Currency = intent.Currency,
            Status = intent.Status,
            Reference = intent.Reference,
            CreatedAt = intent.CreatedAt,
        };
    }
}
