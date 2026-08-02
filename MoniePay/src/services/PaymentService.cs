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
        public async Task<PaymentResponse> SettleIntent(PaymentIntents intents, Guid? paymentMethodId)
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

            // retrieve the reciever details
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
