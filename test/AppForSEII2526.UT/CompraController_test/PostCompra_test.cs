using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class PostPurchase_test : AppForSEII25264SqliteUT
    {
        public PostPurchase_test()
        {
            // === Seed con tus datos ===

            var model = new Model
            {
                NameModel = "ThinkPad"
            };

            var device = new Device
            {
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
                // Importante: el controlador busca por Name y Surname
                UserName = "jaime.rodiguez7@uclm.es", // login
                Name = "jaime.rodiguez7@uclm.es",     // se usa en el DTO como Name
                Surname = "Rodriguez de Vera"
            };

            _context.Model.Add(model);
            _context.Device.Add(device);
            _context.ApplicationUser.Add(user);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_OK_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchasesController>>();
            var controller = new PurchasesController(_context, mockLogger.Object);

            var dto = new PurchaseForCreateDTO(
                name: "jaime.rodiguez7@uclm.es",
                surname: "Rodriguez de Vera",
                deliveryAddress: "Avda. España s/n, Albacete",
                paymentMethod: PaymentMethodTypes.CreditCard,
                purchaseDate: DateTime.Today,
                purchaseDevices: new List<PurchaseDeviceDTO>
                {
                    new PurchaseDeviceDTO(
                        deviceID: 1,          // Id del device en BD (si es Identity será 1)
                        purchasePrice: 0m,    // el controlador usará PriceForPurchase=2000
                        brand: "Lenovo",
                        model: "ThinkPad",
                        quantity: 1)
                }
            );

            // Act
            var result = await controller.CreatePurchase(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var purchaseDetail = Assert.IsType<PurchaseForDetailDTO>(created.Value);

            Assert.Equal("jaime.rodiguez7@uclm.es", purchaseDetail.Name);
            Assert.Equal("Rodriguez de Vera", purchaseDetail.Surname);
            Assert.Equal("Avda. España s/n, Albacete", purchaseDetail.DeliveryAddress);
            Assert.Equal(PaymentMethodTypes.CreditCard, purchaseDetail.PaymentMethod);
            Assert.Equal(2000m, purchaseDetail.TotalPrice);

            Assert.Single(purchaseDetail.PurchaseDevices);
            var item = purchaseDetail.PurchaseDevices.First();
            Assert.Equal("Lenovo", item.Brand);
            Assert.Equal("ThinkPad", item.Model);
            Assert.Equal(2000m, item.PurchasePrice);
            Assert.Equal(1, item.Quantity);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_UserNotFound_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchasesController>>();
            var controller = new PurchasesController(_context, mockLogger.Object);

            var dto = new PurchaseForCreateDTO(
                name: "no.existe@uclm.es",         // usuario que NO está en BD
                surname: "Otro Usuario",
                deliveryAddress: "Calle Sol 10, Toledo",
                paymentMethod: PaymentMethodTypes.CreditCard,
                purchaseDate: DateTime.Today,
                purchaseDevices: new List<PurchaseDeviceDTO>
                {
                    // Device existente, para que el único error sea el usuario
                    new PurchaseDeviceDTO(
                        deviceID: 1,
                        purchasePrice: 0m,
                        brand: "Lenovo",
                        model: "ThinkPad",
                        quantity: 1)
                }
            );

            // Act
            var result = await controller.CreatePurchase(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            var error = problemDetails.Errors.First().Value[0];

            Assert.Equal("Error! User not found", error);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_DeviceNotFound_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchasesController>>();
            var controller = new PurchasesController(_context, mockLogger.Object);

            var dto = new PurchaseForCreateDTO(
                name: "jaime.rodiguez7@uclm.es",
                surname: "Rodriguez de Vera",
                deliveryAddress: "Calle Falsa 123",
                paymentMethod: PaymentMethodTypes.CreditCard,
                purchaseDate: DateTime.Today,
                purchaseDevices: new List<PurchaseDeviceDTO>
                {
                    // Modelo que NO existe en la BD
                    new PurchaseDeviceDTO(
                        deviceID: 999,
                        purchasePrice: 0m,
                        brand: "Lenovo",
                        model: "ModeloInexistente",
                        quantity: 1)
                }
            );

            // Act
            var result = await controller.CreatePurchase(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            var error = problemDetails.Errors.First().Value[0];

            Assert.Equal("Error! Device named 'ModeloInexistente' does not exist in the database", error);
        }
    }
}
