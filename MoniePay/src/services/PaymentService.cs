using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;


namespace MoniePay.src.services
{
    public class PaymentService
    {
        private readonly AppDbContext db;
        public PaymentService(AppDbContext db)
        {
            this.db = db;
        }

        // function to settle the intent
        public async Task<PaymentResponse> SettleIntent(PaymentIntents intents)
        {

            // check if the money is already done 
            var existing_paymentid = await db.Payment.FirstOrDefaultAsync(x => x.PaymentIntentId == intents.Id && x.IsFinal);
            if (existing_paymentid != null)
            {
                return PaymentResponse.From(existing_paymentid);
            }

            // retrieve the account of customer if not done
            var source = await db.LedgerAccount.FirstOrDefaultAsync(
                so => so.customerId == intents.CustomerId && so.Currency == intents.Currency)
                 ?? throw new InvalidOperationException($" No {intents.Currency} account found for this customer {intents.CustomerId}");

            // match funds and check
            if (source.Balance < intents.Amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            // resolve destination 
            var destination = await ResolveSettlementAccount(intents);
            await using var tx = await db.Database.BeginTransactionAsync();
            try
            {
                // everything below will be our transaction 

                var payment = new Payments
                {
                    Id = Guid.NewGuid(),
                    PaymentIntentId = intents.Id,
                    Provider = "Moniepay",
                    ProviderReference = $"MP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..24],
                    Status = Status.PROCESSING,
                    Channel = intents.channel,
                    Attempt = await db.Payment.CountAsync(p => p.PaymentIntentId == intents.Id) + 1,
                    IsFinal = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                // Without this the row is never inserted, and the ledger entries
                // below reference a payment that does not exist.
                db.Payment.Add(payment);

                // Link the intent to its payment so callers see the navigation.
                intents.Payment = payment;

                //Enter the entry into both ledgers of the source and destination
                db.LedgerEntries.AddRange(
                    new LedgerEntries(Guid.NewGuid(), source.Id, -intents.Amount, "DEBIT", payment.Id, DateTime.UtcNow),
                    new LedgerEntries(Guid.NewGuid(), destination.Id, intents.Amount, "CREDIT", payment.Id, DateTime.UtcNow)
                    );

                // update source balance
                source.Balance -= intents.Amount;
                destination.Balance += intents.Amount;

                // mark payment has done
                payment.Status = Status.SUCCESS;
                payment.IsFinal = true;
                intents.Status = Status.SUCCESS;
                intents.UpdatedAt = DateTime.UtcNow;

                await db.SaveChangesAsync();
                await tx.CommitAsync();

                return PaymentResponse.From(payment);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }

        }

        private async Task<LedgerAccounts> ResolveSettlementAccount(PaymentIntents intent)
        {
            var destination_account = await db.LedgerAccount.FirstOrDefaultAsync(
                a => a.AccountNumber == intent.DestinationAccountNumber && a.Currency == intent.Currency)
                ?? throw new InvalidOperationException($"Account {intent.DestinationAccountNumber} not found");

            if (destination_account.customerId == intent.CustomerId)
                throw new InvalidOperationException("Cannot send a payment to yourself");

            // currency must match, mixing 
            if (destination_account.Currency != intent.Currency)
                throw new InvalidOperationException($"Account {intent.DestinationAccountNumber} " +
                    $"is {destination_account.Currency}, not {intent.Currency}");

            // get the customer
            var reciever = await db.Customers.FirstOrDefaultAsync
                (c => c.Id == destination_account.customerId) ?? throw new InvalidOperationException(
                    "Receiver not found");

            // check if user is not blacklisted
            if (reciever.IsBlackListed) throw new InvalidOperationException("reciever is blacklisted");
            if (!reciever.IsVerifed) throw new InvalidOperationException("reciever is not verified");

            return destination_account;

        }

    }
}
