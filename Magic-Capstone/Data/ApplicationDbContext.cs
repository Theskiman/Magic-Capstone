using System;
using System.Collections.Generic;
using System.Text;
using Magic_Capstone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Magic_Capstone.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
            public DbSet<Card> cards { get; set; }
            public DbSet<Deck> decks { get; set;}
            public DbSet<CardDeck> cardDecks { get; set; }
            public DbSet<Condition> conditions { get; set; }
        public DbSet<CardData> cardDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Deck>()
                .HasMany(o => o.cardDecks)
                .WithOne(l => l.Deck)
                .OnDelete(DeleteBehavior.Restrict);


            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Clifton",
                LastName = "Matuszewski",
                StreetAddress = "123 address street",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Clifmatski1!");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            modelBuilder.Entity<Condition>().HasData(
                new Condition()
                {
                    ConditionId = 1,
                    ConitionName = "Mint"
                },

                new Condition()
                {
                    ConditionId = 2,
                    ConitionName = "Near Mint"
                },

                new Condition()
                {
                    ConditionId = 3,
                    ConitionName = "Played"
                },

                new Condition()
                {
                    ConditionId = 4,
                    ConitionName = "Damaged"
                });

            modelBuilder.Entity<Card>().HasData(
                new Card()
                {
                    CardId = 1,
                    name = "Academy Researchers",
                    
                    text = "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.",
                    manaCost = "{1}{U}{U}",
                    
                    type = "Creature — Human Wizard",
                    rarity = "Uncommon",
                    UserId = "00000000-ffff-ffff-ffff-ffffffffffff"
                    


                });
            modelBuilder.Entity<Deck>().HasData(
                new Deck()
                {
                    DeckId = 1,
                    DeckName = "Clifs First Deck",
                    Description = "First test deck",
                    UserId = "00000000-ffff-ffff-ffff-ffffffffffff"
                });

            modelBuilder.Entity<CardDeck>().HasData(
                new CardDeck()
                {
                    CardDeckId = 1,
                    CardDataId = 1,
                    DeckId = 1
                });
        }
        public DbSet<Magic_Capstone.Models.Rootobject> Rootobject { get; set; }
        
    }
}
