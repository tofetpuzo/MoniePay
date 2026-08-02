using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public interface IPaymentIntentService
    {
        Task<PaymentIntentResponse> CreatePaymentIntent(CreatePaymentIntentRequest? request);
    }

    public class PaymentIntentService : IPaymentIntentService
    {
        private readonly ICustomerService customerService;
        private readonly PaymentService paymentService;
        private readonly AppDbContext db;

        public PaymentIntentService(ICustomerService service,
            PaymentService paymentService, AppDbContext db)
        {
            this.customerService = service;
            this.paymentService = paymentService;
            this.db = db;
        }

        public async Task<PaymentIntentResponse> CreatePaymentIntent(CreatePaymentIntentRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(request));

            // if the user creates the same payment intent twice
            var idm_replay = await db.PaymentIntent.FirstOrDefaultAsync(i => i.CustomerId == request.customerId
                                                                                && i.IdempotencyKey == request.IdempotencyKey);

            if (idm_replay is not null)
            {
                return PaymentIntentResponse.From(idm_replay);
            }

            // Awaited, not .Wait() - blocking on a Task here would deadlock under load.
            var customer = await customerService.GetCustomer(request.customerId)
                ?? throw new InvalidOperationException($"Customer {request.customerId} not found");

            // Server owns the identity and the timestamps; the caller never supplies them.
            var intent = new PaymentIntents(
                id: Guid.NewGuid(),
                customerId: customer.Id,
                amount: request.Amount,
                currency: request.currency.ToString(),
                idempotencyKey: request.IdempotencyKey,
                reference: request.Reference,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow)
            {
                Channel = request.channel,
                Status = Status.PROCESSING
            };

            // No money moves here. This only records that a payment is intended.
            // The ledger is touched by PaymentService when POST /payments runs.

            // Mapped to a DTO so the entity's navigations never reach the wire.
            return PaymentIntentResponse.From(intent);

            // TODO: persist via DbContext, enforce idempotency on (customerId, IdempotencyKey),
            //       and check currency matches the customer's ledger account currency.
        }
    }
}
