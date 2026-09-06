using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public interface IPaymentIntentService
    {
        /// <param name="authenticatedUserId">
        /// Id of the caller, taken from the JWT - not from the request body.
        /// The intent is only created if this user owns request.customerId.
        /// </param>
        Task<PaymentIntentResponse> CreatePaymentIntent(CreatePaymentIntentRequest? request, Guid authenticatedUserId);
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

        public async Task<PaymentIntentResponse> CreatePaymentIntent(CreatePaymentIntentRequest? request,
            Guid authenticatedUserId)
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

            // customerId comes from the request body, so it is attacker-controlled.
            // Without this an authenticated user could debit anyone's account.
            // A customer with no owner is never payable.
            if (customer.UserId is null || customer.UserId != authenticatedUserId)
                throw new UnauthorizedAccessException(
                    $"Customer {request.customerId} does not belong to the authenticated user");

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
                channel = request.channel,
                Status = Status.PROCESSING,
                // Without these, ResolveSettlementAccount looks up an empty
                // account number and every settlement fails.
                DestinationAccountNumber = request.DestinationAccountNumber,
                DestinationAccountName = request.DestinationAccountName
            };

            // Saved BEFORE settlement, for two reasons: the idempotency lookup
            // above can only find a replay if the row exists, and the payment
            // row SettleIntent inserts carries a FK to this intent.
            // Deliberately outside the settlement transaction - a failed
            // settlement should still leave a PROCESSING intent to retry.
            db.PaymentIntent.Add(intent);
            await db.SaveChangesAsync();

            // confirm = false means create the intent only; settle later.
            // Checked before settling, or the flag does nothing.
            if (!request.Confirm)
                return PaymentIntentResponse.From(intent);

            var payment = await paymentService.SettleIntent(intent);

            // Mapped to a DTO so the entity's navigations never reach the wire.
            return PaymentIntentResponse.From(intent, payment);

        }

        // TODO: pay deposit and withdrawal.
        // TODO: payout implementation - from one merchant to another.
        // TODO: deposit implementation
    }
}
