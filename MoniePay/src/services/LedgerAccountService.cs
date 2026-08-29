using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public class LedgerAccountService
    {

        private readonly AppDbContext db;
        public LedgerAccountService(AppDbContext dbContext)
        {
            this.db = dbContext;

        }
        public async Task<LedgerAccounts> ledgerAccounts(Guid customerId)
        {

            try
            {
                var account = await db.LedgerAccount.FirstOrDefaultAsync(a => a.customerId == customerId) ?? throw new KeyNotFoundException("cannot find customer");
                return account;
            }
            catch
            {
                throw new KeyNotFoundException("account detail not found");
            }

        }

        public static Task<string> AddCheckDigitAsync(string number)
        {
            if (string.IsNullOrWhiteSpace(number) ||
                !ulong.TryParse(number, out _))
            {
                throw new ArgumentException("The number must contain digits only.");
            }

            for (int checkDigit = 0; checkDigit <= 9; checkDigit++)
            {
                string candidate = number + checkDigit;

                if (IsValid(candidate))
                    return Task.FromResult(candidate);
            }

            throw new InvalidOperationException("Unable to generate a check digit.");
        }

        public static Task<bool> IsValidAsync(string number)
        {
            return Task.FromResult(IsValid(number));
        }

        private static bool IsValid(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            int sum = 0;
            bool doubleDigit = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(number[i]))
                    return false;

                int digit = number[i] - '0';

                if (doubleDigit)
                {
                    digit *= 2;

                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }


        public async Task<string> NextAccountNumberAsync()
        {
            long nextNumber = await db.Database
                                 .SqlQuery<long>($"SELECT nextval('account_number_seq') AS \"Value\"")
                                 .SingleAsync();

            return await AddCheckDigitAsync(nextNumber.ToString("D12"));
        }
    }
}
