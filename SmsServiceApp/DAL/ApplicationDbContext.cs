using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCustomerApp.Models;

namespace WebCustomerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();


        }

        public DbSet<Code> Codes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<StopWord> StopWords { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Explicitly setting PK:

            builder.Entity<Code>().HasKey(i => i.Id);
            builder.Entity<Company>().HasKey(i => i.Id);
            builder.Entity<Contact>().HasKey(i => i.Id);
            builder.Entity<Operator>().HasKey(i => i.Id);
            builder.Entity<Recipient>().HasKey(i => i.Id);
            builder.Entity<Phone>().HasKey(i => i.Id);
            builder.Entity<Tariff>().HasKey(i => i.Id);
            builder.Entity<StopWord>().HasKey(i => i.Id);

            // Compound key for Many-To-Many joining table

            // Setting FK
            #region FK

            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Contacts)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Companies)
                .WithOne(com => com.ApplicationUser)
                .HasForeignKey(com => com.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<Operator>()
                .HasMany(o => o.Codes)
                .WithOne(c => c.Operator)
                .HasForeignKey(c => c.OperatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Operator>()
                .HasMany(o => o.Tariffs)
                .WithOne(t => t.Operator)
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Phone>()
                .HasMany(p => p.Contacts)
                .WithOne(c => c.Phone)
                .HasForeignKey(c => c.PhoneId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<Tariff>()
                .HasMany(t => t.Companies)
                .WithOne(com => com.Tariff)
                .HasForeignKey(com => com.TariffId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            #endregion

            // Configuring Many-To-Many relationship through Recipient and compound index

            builder.Entity<Company>()
                .HasMany(c => c.Recipients)
                .WithOne(r => r.Company)
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Phone>()
                .HasMany(p => p.Recipients)
                .WithOne(r => r.Phone)
                .HasForeignKey(r => r.PhoneId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Recipient>()
                .HasIndex(r => new { r.PhoneId, r.CompanyId })
                .IsUnique();

            // Required fields
            #region Required fields
            builder.Entity<Code>()
                .Property(c => c.OperatorCode)
                .IsRequired();

            builder.Entity<Company>()
                .Property(com => com.Name)
                .IsRequired();

            builder.Entity<Company>()
                .Property(com => com.Message)
                .IsRequired();

            builder.Entity<Phone>()
                .Property(p => p.PhoneNumber)
                .IsRequired();

            builder.Entity<Tariff>()
                .Property(t => t.Name)
                .IsRequired();

            builder.Entity<Tariff>()
                .Property(t => t.Price)
                .IsRequired();

            builder.Entity<Tariff>()
                .Property(t => t.Limit)
                .IsRequired();

            builder.Entity<Operator>()
                .Property(o => o.Name)
                .IsRequired();

            builder.Entity<StopWord>()
                .Property(sw => sw.Word)
                .IsRequired();

            #endregion

            // Unique indexes

            builder.Entity<Operator>()
                .HasIndex(o => o.Name)
                .IsUnique();

            builder.Entity<Code>()
                .HasIndex(i => i.OperatorCode)
                .IsUnique();
        }
    }
}
