using Microsoft.EntityFrameworkCore;
using MoniePay.src.auth;
using MoniePay.src.models;

namespace MoniePay.src.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EventStore> EventStores { get; set; }
        public DbSet<LedgerAccounts> LedgerAccount { get; set; }
        public DbSet<LedgerEntries> LedgerEntries { get; set; }
        public DbSet<PaymentAttempts> PaymentAttempt { get; set; }
        public DbSet<PaymentIntents> PaymentIntent { get; set; }
        public DbSet<PaymentMethods> PaymentMethod { get; set; }
        public DbSet<Payments> Payment { get; set; }
        public DbSet<Payouts> Payout { get; set; }
        public DbSet<Transactions> Transaction { get; set; }
        public DbSet<WebhookDeliveries> WebhookDeliveries { get; set; }
        public DbSet<Webhooks> Webhook { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
