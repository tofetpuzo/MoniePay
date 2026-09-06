
// SPDX-License-Identifier: Apache-2.0
/*
 * EF Core database context wiring ASP.NET Identity to the domain model.
 *
 * Copyright (c) 2026, MoniePay
 */

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
        // Named AppRoles, not Roles: IdentityDbContext already exposes a
        // Roles property (DbSet<IdentityRole<Guid>>). Declaring another one
        // hid it (CS0114) and made db.Roles ambiguous to read. The table is
        // still "Roles" - see ToTable below - so no migration is needed.
        public DbSet<Roles> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map ASP.NET Identity tables to friendlier names.
            // NOTE: IdentityRole<Guid> is left as "IdentityRoles" to avoid clashing
            // with the custom Roles entity (MoniePay.src.auth.Roles -> "Roles" table).
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("IdentityRoles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

            // Bind Customer.UserId to the column EF previously created as a
            // shadow FK, so making it explicit is not a schema change.
            builder.Entity<Customer>()
                .Property(c => c.UserId)
                .HasColumnName("userId");

            builder.Entity<Customer>()
                .HasOne(c => c.user)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId);

            // Keeps the table named "Roles" even though the DbSet is AppRoles,
            // so renaming the property does not produce a schema change.
            builder.Entity<Roles>().ToTable("Roles");

            // One user -> many role rows, linked by Roles.UserId.
            builder.Entity<Roles>()
                .HasOne(r => r.User)
                .WithMany(u => u.roles)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // this is for account generation in the database
            builder.HasSequence<long>("account_number_seq")
                .StartsAt(100_000_000_000L)
                .IncrementsBy(1);

            // LedgerEntries.AccountId is the FK for the LedgerAccount navigation.
            // Without this EF invents a second, nullable shadow column
            // (LedgerAccountId) and AccountId is left as an unenforced duplicate.
            builder.Entity<LedgerEntries>()
                .HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.AccountId);

            // defense in depth refuses duplicates 
            builder.Entity<LedgerAccounts>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            // add transaction relationship with LedgerEntries
            builder.Entity<Transactions>()
                .HasMany(t => t.LedgerEntries)
                .WithOne(e => e.Transaction)
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
