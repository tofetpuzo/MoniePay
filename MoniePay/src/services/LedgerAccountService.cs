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
                var account = await db.FindAsync<LedgerAccounts>(customerId) ?? throw new KeyNotFoundException("cannot find customer");
                return account;
            }
            catch
            {
                throw new KeyNotFoundException("account detail not found");
            }

        }
    }
}
