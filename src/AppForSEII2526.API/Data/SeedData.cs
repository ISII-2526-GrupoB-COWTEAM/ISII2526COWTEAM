using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context, IServiceProvider serviceProvider, ILogger logger)
        {
            if (!context.ApplicationUser.Any())
            {
                 logger.LogInformation("Seeding database (totally empty)...");
            }

            // Ensure Carlos exists and has correct data
            var carlos = context.ApplicationUser.FirstOrDefault(u => u.Email == "carlos@test.com");
            if (carlos == null)
            {
                carlos = new ApplicationUser
                {
                    Name = "Carlos",
                    Surname = "García Fernández",
                    Country = "Spain",
                    UserName = "carlos@test.com",
                    NormalizedUserName = "CARLOS@TEST.COM",
                    Email = "carlos@test.com",
                    NormalizedEmail = "CARLOS@TEST.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                context.ApplicationUser.Add(carlos);
            }
            else
            {
                // Force update surname if it doesn't match
                if (carlos.Surname != "García Fernández")
                {
                     carlos.Surname = "García Fernández";
                     context.Update(carlos);
                }
            }

            // Ensure Laura exists
            if (!context.ApplicationUser.Any(u => u.Email == "laura@test.com"))
            {
                var user2 = new ApplicationUser
                {
                    Name = "Laura",
                    Surname = "Martínez",
                    Country = "Spain",
                    UserName = "laura@test.com",
                    NormalizedUserName = "LAURA@TEST.COM",
                    Email = "laura@test.com",
                    NormalizedEmail = "LAURA@TEST.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                context.ApplicationUser.Add(user2);
            }
            
           context.SaveChanges();

            // SAMPLES (Scales)
            if (!context.Scale.Any()) {
                var scale1 = new Scale { Id = 1, Name = "Low" };
                var scale2 = new Scale { Id = 2, Name = "Medium" };
                var scale3 = new Scale { Id = 3, Name = "High" };
                context.Scale.AddRange(scale1, scale2, scale3);
            }

            // MODELS
            if (!context.Model.Any()) {
                var model1 = new Model { Id = 1, NameModel = "iPhone 14" };
                var model2 = new Model { Id = 2, NameModel = "Samsung Galaxy S23" };
                var model3 = new Model { Id = 3, NameModel = "MacBook Pro" };
                var model4 = new Model { Id = 4, NameModel = "Dell XPS 15" };
                context.Model.AddRange(model1, model2, model3, model4);
            }

            context.SaveChanges();

            // DEVICES
            var device1 = new Device
            {
                Id = 1,
                Brand = "Apple",
                Color = "Black",
                Description = "High-end smartphone",
                Name = "iPhone 14",
                PriceForPurchase = 999,
                PriceForRent = 25,
                Quality = "New",
                QuantityForPurchase = 10,
                QuantityForRent = 5,
                Year = 2023,
                
            };

            var device2 = new Device
            {
                Id = 2,
                Brand = "Samsung",
                Color = "White",
                Description = "Android flagship phone",
                Name = "Galaxy S23",
                PriceForPurchase = 899,
                PriceForRent = 22,
                Quality = "New",
                QuantityForPurchase = 8,
                QuantityForRent = 6,
                Year = 2023,
                
            };

            var device3 = new Device
            {
                Id = 3,
                Brand = "Apple",
                Color = "Gray",
                Description = "Professional laptop",
                Name = "MacBook Pro",
                PriceForPurchase = 2200,
                PriceForRent = 50,
                Quality = "New",
                QuantityForPurchase = 4,
                QuantityForRent = 2,
                Year = 2022,
                
            };

            var device4 = new Device
            {
                Id = 4,
                Brand = "Dell",
                Color = "Silver",
                Description = "Powerful Windows laptop",
                Name = "XPS 15",
                PriceForPurchase = 1800,
                PriceForRent = 45,
                Quality = "New",
                QuantityForPurchase = 6,
                QuantityForRent = 3,
                Year = 2022,
               
            };

            context.Device.AddRange(device1, device2, device3, device4);
            
            // REPAIRS
            context.Repair.AddRange(
                new Repair { Id = 1, Cost = 50, Description = "Screen replacement", Name = "Screen repair", ScaleId = 2 },
                new Repair { Id = 2, Cost = 80, Description = "Battery replacement", Name = "Battery repair", ScaleId = 1 },
                new Repair { Id = 3, Cost = 150, Description = "Motherboard fix", Name = "Hardware repair", ScaleId = 3 }
            );

            context.SaveChanges();

            // Purchases, Rentals, Receipts, Reviews could be added here if needed.
            // For now, Devices and Users are the critical dependency for purchase tests.
        }
    }
}
