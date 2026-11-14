// SPDX-License-Identifier: Apache-2.0
/*
 * Banking Funding service for customers and cash transfer via cashier 
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public class BankFundingService(AppDbContext dbContext, BankLedgerPostingService postingService)
    {
        private readonly AppDbContext _db = dbContext;
        private readonly BankLedgerPostingService _postingService = postingService;

        // record amount pay to cashier
        public async Task<BankFundingResponse> RecordFunding(BankingFundingRequest request, Guid recordBy)
        {
            ArgumentNullException.ThrowIfNull(request);

            // check the banking reference to avoid duplicate
            var existing = await _db.BankFunding.FirstOrDefaultAsync(f => f.BankReference == request.BankingReference);

            if (existing != null) return BankFundingResponse.From(existing);

            // find the bank for funding from the request
            var bank = await _db.BankAccount.FindAsync(request.BankAccountId) ?? throw new KeyNotFoundException($"Bank account {request.BankAccountId} not found");

            // check if the bank is active
            if (!bank.IsActive) throw new InvalidOperationException("Bank account is active");

            var funding = new BankFundings
            {
                Id = Guid.NewGuid(),
                BankAccountId = bank.Id,
                Amount = request.Amount,
                Currency = request.Currency,
                BankReference = request.BankingReference,
                Narration = request.Narration,
                Status = Status.PROCESSING,
                RecordedBy = recordBy,
                CreatedAt = DateTime.UtcNow,
            };

            _db.BankFunding.Add(funding);
            await _db.SaveChangesAsync();

            return BankFundingResponse.From(funding);
        }

        public async Task<BankFundingResponse> PostFunding(Guid fundingId, Guid confirmedBy)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var funding = await _db.BankFunding
                    .Include(f => f.BankAccount)
                    .FirstOrDefaultAsync(f => f.Id == fundingId)
                    ?? throw new KeyNotFoundException($"Funding {fundingId} not found");

                if (funding.Status == Status.SUCCESS) return BankFundingResponse.From(funding);
                if (funding.Status != Status.PROCESSING) throw new InvalidOperationException($"Funding is {funding.Status} cannot be confirmed");

                // check who recorded the cash transfer

                if (funding.RecordedBy == confirmedBy) throw new UnauthorizedAccessException("The recorder cant be the one who confirmed the transfer");

                var bankLedger = await _db.LedgerAccount.FindAsync(funding.BankAccount!.LedgerAccountId)
                                       ?? throw new InvalidOperationException("Bank ledger account missing");

                //check the ledger for the account type
                var capital = await _db.LedgerAccount.FirstOrDefaultAsync
                    (a => a.AccountType == AccountType.Capital && a.Currency == funding.Currency)
                    ?? throw new KeyNotFoundException($"No {funding.Currency} capital account exists");


                var transaction = _postingService.Post
                    (
                     TransactionType.BankFunding,
                     debitAccount: capital,
                     creditAccount: bankLedger,
                     amount: funding.Amount,
                     reference: funding.BankReference,
                     initiateBy: confirmedBy,
                     description: $"Bank funding {funding.BankReference}");

                // mark transaction to success
                funding.Status = Status.SUCCESS;
                funding.ConfirmedBy = confirmedBy;
                funding.ConfirmedAt = DateTime.UtcNow;
                funding.TransactionId = tx.TransactionId;

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return BankFundingResponse.From(funding);
            }
            catch
            {
                await tx.RollbackAsync(); throw;
            }
        }

        public async Task<Guid> TransferToVaultAsync(TransferToVaultRequest request, Guid initiateBy)
        {
            var replay = await _db.Transaction.FirstOrDefaultAsync(t =>
            t.Type == TransactionType.VaultFunding && t.Reference == request.IdempotencyKey);

            if (replay != null) return replay.Id;

            await using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var bank = await _db.BankAccount.FindAsync(request.BankAccountId)
                    ?? throw new KeyNotFoundException($"Bank account {request.BankAccountId} not found");

                var bankLedger = await _db.LedgerAccount.FindAsync(bank.LedgerAccountId)
                                      ?? throw new InvalidOperationException("Bank ledger account missing");

                var vault = await _db.LedgerAccount
                    .FirstOrDefaultAsync(a => a.AccountType == AccountType.Vault && a.Currency == bank.Currency)
                    ?? throw new KeyNotFoundException($"No {bank.Currency} vault exists");

                var transaction = _postingService.Post
                    (
                    TransactionType.VaultFunding,
                    debitAccount: bankLedger,
                    creditAccount: vault,
                    amount: request.Amount,
                    reference: request.IdempotencyKey,
                    initiateBy: initiateBy,
                    description: $"Caah withdraw from {bank.BankName} to Vault"
                    );

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return transaction.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
