using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoniePay.src.auth;
using MoniePay.src.models;

namespace MoniePay.src.data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map ASP.NET Identity tables to friendlier names.
            // NOTE: IdentityRole<Guid> is left as "IdentityRoles" to avoid clashing
            // with your custom Roles DbSet (MoniePay.src.auth.Roles -> "Roles" table).
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("IdentityRoles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        }
    }
}
