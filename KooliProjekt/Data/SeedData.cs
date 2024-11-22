using Microsoft.AspNetCore.Identity;
using KooliProjekt.Data;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.Ingredients.Any() || context.Beers.Any() || context.Batches.Any() || context.LogEntries.Any() || context.Photos.Any() || context.TastingEntries.Any())
            {
                return;
            }

            var user = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
            };
            userManager.CreateAsync(user, "Password123!").Wait();            

            var beers = new Beer[]
            {
                new Beer { Name = "Amber Ale", Description = "Rich, malty ale" },
                new Beer { Name = "Pale Ale", Description = "Crisp, bitter ale" },
                new Beer { Name = "Stout", Description = "Dark, thick stout" },
                new Beer { Name = "IPA", Description = "Hoppy, intense IPA" },
                new Beer { Name = "Wheat Beer", Description = "Light, refreshing wheat beer" },
                new Beer { Name = "Pilsner", Description = "Crisp, refreshing pilsner" },
                new Beer { Name = "Porter", Description = "Rich, roasted porter" },
                new Beer { Name = "Lager", Description = "Light, smooth lager" },
                new Beer { Name = "Saison", Description = "Fruity, spicy farmhouse ale" },
                new Beer { Name = "Bock", Description = "Strong, malty bock beer" }
            };
            context.Beers.AddRange(beers);

            var batches = new Batch[]
            {
                new Batch { Date = DateTime.Now.AddDays(-10), Code = "BATCH001", Description = "Batch of Pale Ale", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-9), Code = "BATCH002", Description = "Batch of IPA", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-8), Code = "BATCH003", Description = "Batch of Stout", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-7), Code = "BATCH004", Description = "Batch of Amber Ale", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-6), Code = "BATCH005", Description = "Batch of Wheat Beer", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-5), Code = "BATCH006", Description = "Batch of Pilsner", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-4), Code = "BATCH007", Description = "Batch of Porter", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-3), Code = "BATCH008", Description = "Batch of Lager", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-2), Code = "BATCH009", Description = "Batch of Saison", Beer = beers[0] },
                new Batch { Date = DateTime.Now.AddDays(-1), Code = "BATCH010", Description = "Batch of Bock", Beer = beers[0] }
            };
            context.Batches.AddRange(batches);


            var user1 = new IdentityUser { UserName = "User1", Email = "user1@example.com" };
            var user2 = new IdentityUser { UserName = "User2", Email = "user2@example.com" };
            var user3 = new IdentityUser { UserName = "User3", Email = "user3@example.com" };

            userManager.CreateAsync(user1, "Password123!").GetAwaiter().GetResult();
            userManager.CreateAsync(user2, "Password123!").GetAwaiter().GetResult();
            userManager.CreateAsync(user3, "Password123!").GetAwaiter().GetResult();

            var logEntries = new LogEntry[]
            {
                new LogEntry { Date = DateTime.Now.AddDays(-10), Description = "Started brewing Pale Ale", User = user1, UserId = user1.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-9), Description = "Started brewing IPA", User = user2, UserId = user2.Id },
                new LogEntry { Date = DateTime.Now.AddDays(-8), Description = "Started brewing Stout", User = user3, UserId = user3.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-7), Description = "Started brewing Amber Ale", User = user1, UserId = user1.Id },
                new LogEntry { Date = DateTime.Now.AddDays(-6), Description = "Started brewing Wheat Beer", User = user2, UserId = user2.Id },
                new LogEntry { Date = DateTime.Now.AddDays(-5), Description = "Started brewing Pilsner", User = user3, UserId = user3.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-4), Description = "Started brewing Porter", User = user1, UserId = user1.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-3), Description = "Started brewing Lager", User = user2, UserId = user2.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-2), Description = "Started brewing Saison", User = user3, UserId = user3.Id  },
                new LogEntry { Date = DateTime.Now.AddDays(-1), Description = "Started brewing Bock", User = user1, UserId = user1.Id  }
            };

            context.LogEntries.AddRange(logEntries);

            var ingredients = new Ingredient[]
            {
                new Ingredient { Name = "Hops", Unit = "kg", UnitPrice = 100, Quantity = 10, AmountUsed = 50, LogEntry = logEntries[0] },
                new Ingredient { Name = "Barley", Unit = "kg", UnitPrice = 80, Quantity = 15, AmountUsed = 60, LogEntry = logEntries[1]  },
                new Ingredient { Name = "Yeast", Unit = "g", UnitPrice = 15, Quantity = 500, AmountUsed = 200, LogEntry = logEntries[2]  },
                new Ingredient { Name = "Water", Unit = "L", UnitPrice = 32, Quantity = 1000, AmountUsed = 500 , LogEntry = logEntries[3] },
                new Ingredient { Name = "Sugar", Unit = "kg", UnitPrice = 120, Quantity = 5, AmountUsed = 2 ,LogEntry = logEntries[4] },
                new Ingredient { Name = "Salt", Unit = "kg", UnitPrice = 50, Quantity = 2, AmountUsed = 1 ,LogEntry = logEntries[5] },
                new Ingredient { Name = "Lemon", Unit = "kg", UnitPrice = 150, Quantity = 3, AmountUsed = 1 ,LogEntry = logEntries[6] },
                new Ingredient { Name = "Malt", Unit = "kg", UnitPrice = 90, Quantity = 12, AmountUsed = 6 ,LogEntry = logEntries[7] },
                new Ingredient { Name = "Ginger", Unit = "kg", UnitPrice = 130, Quantity = 8, AmountUsed = 3 ,LogEntry = logEntries[0] },
                new Ingredient { Name = "Coriander", Unit = "kg", UnitPrice = 110, Quantity = 6, AmountUsed = 2 ,LogEntry = logEntries[0] }
            };
            context.Ingredients.AddRange(ingredients);

            var photos = new Photo[]
            {
                new Photo { Url = "https://example.com/photo1.jpg", Description = "Pale Ale Brew Process" },
                new Photo { Url = "https://example.com/photo2.jpg", Description = "IPA Brew Process" },
                new Photo { Url = "https://example.com/photo3.jpg", Description = "Stout Brew Process" },
                new Photo { Url = "https://example.com/photo4.jpg", Description = "Amber Ale Brew Process" },
                new Photo { Url = "https://example.com/photo5.jpg", Description = "Wheat Beer Brew Process" },
                new Photo { Url = "https://example.com/photo6.jpg", Description = "Pilsner Brew Process" },
                new Photo { Url = "https://example.com/photo7.jpg", Description = "Porter Brew Process" },
                new Photo { Url = "https://example.com/photo8.jpg", Description = "Lager Brew Process" },
                new Photo { Url = "https://example.com/photo9.jpg", Description = "Saison Brew Process" },
                new Photo { Url = "https://example.com/photo10.jpg", Description = "Bock Brew Process" }
            };
            context.Photos.AddRange(photos);

            // Проверьте, есть ли уже данные в базе
            if (context.TastingEntries.Any())
            {
                return;
            }

            var tastingEntries = new TastingEntry[]
            {
                new TastingEntry { Date = DateTime.Now.AddDays(-10), Rating = 5, Comments = "Excellent Pale Ale", BatchId = 1 },
                new TastingEntry { Date = DateTime.Now.AddDays(-9), Rating = 4, Comments = "Great IPA", BatchId = 2 },
                new TastingEntry { Date = DateTime.Now.AddDays(-8), Rating = 5, Comments = "Amazing Stout", BatchId = 3 },
                new TastingEntry { Date = DateTime.Now.AddDays(-7), Rating = 4, Comments = "Good Amber Ale", BatchId = 4 },
                new TastingEntry { Date = DateTime.Now.AddDays(-6), Rating = 4, Comments = "Nice Wheat Beer", BatchId = 5 },
                new TastingEntry { Date = DateTime.Now.AddDays(-5), Rating = 3, Comments = "Okay Pilsner", BatchId = 6 },
                new TastingEntry { Date = DateTime.Now.AddDays(-4), Rating = 5, Comments = "Great Porter", BatchId = 7 },
                new TastingEntry { Date = DateTime.Now.AddDays(-3), Rating = 4, Comments = "Good Lager", BatchId = 8 }
            };

            context.TastingEntries.AddRange(tastingEntries);

            context.SaveChanges();
        }
    }
}
