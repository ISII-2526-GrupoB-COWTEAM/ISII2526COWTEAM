using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
        {
            

            var model = new Model
            {
                NameModel = "ThinkPad"
            };

            var device = new Device
            {
                // Id se genera por la BD si es Identity, no hace falta fijarlo
                Name = "ThinkPad",
                Brand = "Lenovo",
                Color = "White",
                Description = "Lenovo ThinkPad test device",
                Quality = "New",
                PriceForPurchase = 2000m,
                Year = 2025,
                Model = model,
                QuantityForPurchase = 10,
                QuantityForRent = 5
            };
            


            var user = new ApplicationUser
            {
                UserName = "jaime.rodiguez7@uclm.es",
                Name = "Jaime",
                Surname = "Rodriguez de Vera"
            };

            var purchase = new Purchase
            {
                ApplicationUser = user,
                DeliveryAddress = "Avda. España s/n, Albacete",
                PaymentMethod = PaymentMethodTypes.CreditCard,
                PurchaseDate = new DateTime(2025, 04, 18),
                TotalPrice = 2000m,
                PurchaseItems = new List<PurchaseItem>()
            };

            var purchaseItem = new PurchaseItem
            {
                Device = device,
                Purchase = purchase,
                Quantity = 1,
                Price = 2000m
            };

            purchase.PurchaseItems.Add(purchaseItem);

            _context.Model.Add(model);
            _context.Device.Add(device);
            _context.ApplicationUser.Add(user);
            _context.Purchase.Add(purchase);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_OK_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchasesController>>();
            var controller = new PurchasesController(_context, mockLogger.Object);
            var purchaseId = _context.Purchase.First().Id;

            // Act
            var result = await controller.GetPurchase(purchaseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PurchaseForDetailDTO>(okResult.Value);

            // Comprobamos datos básicos
            Assert.Equal(purchaseId, dto.Id);
            Assert.Equal("jaime.rodiguez7@uclm.es", dto.Name);
            Assert.Equal("Rodriguez de Vera", dto.Surname);
            Assert.Equal("Avda. España s/n, Albacete", dto.DeliveryAddress);
            Assert.Equal(PaymentMethodTypes.CreditCard, dto.PaymentMethod);
            Assert.Equal(2000m, dto.TotalPrice);

            // Comprobamos el device
            Assert.Single(dto.PurchaseDevices);
            var deviceDto = dto.PurchaseDevices.First();
            Assert.Equal("Lenovo", deviceDto.Brand);
            Assert.Equal("ThinkPad", deviceDto.Model);
            Assert.Equal(2000m, deviceDto.PurchasePrice);
            Assert.Equal(1, deviceDto.Quantity);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchasesController>>();
            var controller = new PurchasesController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetPurchase(999);

            // Assert
           
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
