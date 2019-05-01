using BankTwoAPI_Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.Data
{
    public class BankTwoDatabase : DbContext
    {
        public BankTwoDatabase(DbContextOptions<BankTwoDatabase> options):base(options)
        {

        }

        public DbSet<AirTimeTopUp> AirTimeTopUp { get; set; }

        public DbSet<Banks> Banks { get; set; }

        public DbSet<CardRequest> CardRequests { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public DbSet<CustomerPassport> CustomerPassports { get; set; }

        public DbSet<AccountCustomerCategory> AccountCustomerCategories { get; set; }

        public DbSet<CustomerBeneficiary> CustomerBeneficiaries { get; set; }

        public DbSet<IntraBankTransfer> IntraBankTransfers { get; set; }

        public DbSet<InterBankTransfer> InterBankTransfers { get; set; }

        public DbSet<MobileNetworks> MobileNetworks { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountCustomerCategory>()
                .HasKey(acc => new {acc.CustomerAccountId,acc.CustomerId });
            modelBuilder.Entity<AccountCustomerCategory>()
                .HasOne(acc => acc.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(acc => acc.CustomerId);
            modelBuilder.Entity<AccountCustomerCategory>()
                .HasOne(acc => acc.CustomerAccount)
                .WithMany(ca => ca.Customers)
                .HasForeignKey(acc => acc.CustomerAccountId);

            //Seeding Banks
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 1, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Access Bank", BankShortName = "Access Bank",BankIdentificationCode = 86428357 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 2, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Fidelity Bank", BankShortName = "Fidelity Bank", BankIdentificationCode = 15090085 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 3, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "First Bank of Nigeria Limited", BankShortName = "First Bank", BankIdentificationCode = 49968453 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 4, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Guaranty Trust Bank Plc", BankShortName = "Guaranty Trust Bank", BankIdentificationCode = 12039523 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 5, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Union Bank for Africa Plc", BankShortName = "Union Bank", BankIdentificationCode = 42359561 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 6, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Zenith Bank Plc", BankShortName = "Zenith Bank", BankIdentificationCode = 60412621 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 7, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Ecobank Nigeria Plc", BankShortName = "Eco Bank", BankIdentificationCode = 49546921 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 8, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Diamond Bank Plc", BankShortName = "Diamond Bank", BankIdentificationCode = 75473128 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 9, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Stanbic IBTC Bank Plc", BankShortName = "Stanbic IBTC Bank", BankIdentificationCode = 58811713 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 10, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Standard Chartered", BankShortName = "Standard Chatered Bank", BankIdentificationCode = 88427198 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 11, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Sterling Bank Plc", BankShortName = "Sterling Bank", BankIdentificationCode = 65002961 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 12, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "First City Monument Bank Plc", BankShortName = "First City Monument Bank", BankIdentificationCode = 48299192 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 13, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Heritage Banking Company Limited", BankShortName = "Heritage Bank", BankIdentificationCode = 15094290 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 14, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Keystone Bank Limited", BankShortName = "Keystone Bank", BankIdentificationCode = 32404055 });
            modelBuilder.Entity<Banks>().HasData(new Banks { Id = 15, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), BankFullName = "Wema Bank Plc", BankShortName = "Wema Bank", BankIdentificationCode = 88387850 });


            //Seeding Mobile Networks
            modelBuilder.Entity<MobileNetworks>().HasData(new MobileNetworks { Id = 1, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), MobileNetworkName = "MTN", NetworkProviderRegistratonCode = 123456789 });
            modelBuilder.Entity<MobileNetworks>().HasData(new MobileNetworks { Id = 2, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), MobileNetworkName = "GLO", NetworkProviderRegistratonCode = 123456789 });
            modelBuilder.Entity<MobileNetworks>().HasData(new MobileNetworks { Id = 3, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), MobileNetworkName = "AIRTEL", NetworkProviderRegistratonCode = 123456789 });
            modelBuilder.Entity<MobileNetworks>().HasData(new MobileNetworks { Id = 4, CreatedAt = DateTime.Parse("2000-01-01 00:00:00"), IsActive = true, LastUpdatedAt = DateTime.Parse("2000-01-01 00:00:00"), MobileNetworkName = "ETISALAT", NetworkProviderRegistratonCode = 123456789 });
           



        }
    }
}
