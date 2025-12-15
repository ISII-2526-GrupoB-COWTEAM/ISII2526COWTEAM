using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
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

namespace AppForSEII2526.UT.DevicesController_test
{
    public class GetDeviceForPurchase_test : AppForSEII25264SqliteUT
    {

        public GetDeviceForPurchase_test()
        {
            var models = new List<Model>()
            {
                new Model { NameModel = "ProBook" },
                new Model { NameModel = "ThinkPad" },
                new Model { NameModel = "Predator" }
            };

            var devices = new List<Device>()
            {
                new Device
                {
                    Brand = "HP",
                    Color = "Negro",
                    Description = "Portátil HP ProBook 450 G9",
                    Quality = "New",
                    Name = "HP ProBook 450",
                    PriceForPurchase = 999.99m,
                    Year = 2024,
                    Model = models[0],
                    QuantityForPurchase = 10,
                    QuantityForRent = 0
                },
                new Device
                {
                    Brand = "Lenovo",
                    Color = "Gris",
                    Description = "Lenovo ThinkPad X1 Carbon",
                    Quality = "New",
                    Name = "Lenovo ThinkPad X1",
                    PriceForPurchase = 1899.99m,
                    Year = 2023,
                    Model = models[1],
                    QuantityForPurchase = 9,
                    QuantityForRent = 0
                },
                new Device
                {
                    Brand = "Lenovo",
                    Color = "Azul",
                    Description = "Lenovo ThinkPad E15",
                    Quality = "New",
                    Name = "Lenovo ThinkPad E15",
                    PriceForPurchase = 699.99m,
                    Year = 2022,
                    Model = models[1],
                    QuantityForPurchase = 8,
                    QuantityForRent = 0
                },
                new Device
                {
                    Brand = "Acer",
                    Color = "Negro",
                    Description = "Acer Predator Helios 300",
                    Quality = "New",
                    Name = "Acer Predator Helios",
                    PriceForPurchase = 1499.99m,
                    Year = 2023,
                    Model = models[2],
                    QuantityForPurchase = 10,
                    QuantityForRent = 0
                }
            };

            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();
        }

        // Caso correcto con distintos filtros
        public static IEnumerable<object[]> TestCasesFor_GetDeviceForPurchase_OK()
        {
            var dtoList = new List<DeviceForPurchaseDTO>()
            {
                new DeviceForPurchaseDTO(5,  "HP ProBook 450",       "Negro",  999.99m,  "ProBook",  "HP", 2025),
                new DeviceForPurchaseDTO(6,  "Lenovo ThinkPad X1",   "Gris",  1899.99m,  "ThinkPad", "Lenovo", 2025),
                new DeviceForPurchaseDTO(9,  "Lenovo ThinkPad E15",  "Azul",   699.99m,  "ThinkPad", "Lenovo", 2025),
                new DeviceForPurchaseDTO(11, "Acer Predator Helios", "Negro", 1499.99m,  "Predator", "Acer", 2025),
            };

            var todos = dtoList.OrderBy(d => d.Name).ThenBy(d => d.Brand).ToList();
            var soloLenovo = dtoList.Where(d => d.Brand == "Lenovo").ToList();
            var soloColor = dtoList.Where(d => d.Colour == "Azul").ToList();
            

            return new List<object[]>
            {
                // sin filtros → todos
                new object[] { null, null, todos },
                // solo color azul → el E15
                new object[] { null, "Azul", soloColor },
                // nombre que contenga "ThinkPad" → uno de Lenovo
                new object[] { "ThinkPad X1", null, new List<DeviceForPurchaseDTO>{ dtoList[1] } },
                
            };
        }


        // Caso de nombre inexistente en la bbdd
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDevicesForPurchase_Empty_WhenNameNotFound_test()
        {
            var mockLogger = new Mock<ILogger<DevicesController>>();
            var controller = new DevicesController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetDevicesForPurchase("Motorola", null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var devices = Assert.IsType<List<DeviceForPurchaseDTO>>(okResult.Value);

            Assert.Empty(devices);
        }

        // Caso de color inexistente en la bbdd
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDevicesForPurchase_Empty_WhenColorNotFound_test()
        {
            var mockLogger = new Mock<ILogger<DevicesController>>();
            var controller = new DevicesController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetDevicesForPurchase(null, "Verde");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var devices = Assert.IsType<List<DeviceForPurchaseDTO>>(okResult.Value);

            Assert.Empty(devices);
        }


        [Theory]
        [MemberData(nameof(TestCasesFor_GetDeviceForPurchase_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDeviceForPurchase_OK_test(string? name, string? color, IList<DeviceForPurchaseDTO> expectedDevices)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DevicesController>>();
            var controller = new DevicesController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetDevicesForPurchase(name, color);

            // Extraemos la lista desde el ActionResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDevices = Assert.IsType<List<DeviceForPurchaseDTO>>(okResult.Value);

            // Ordenamos ambas listas antes de comparar (por Name y Brand)
            var expectedOrdered = expectedDevices.OrderBy(d => d.Name).ThenBy(d => d.Brand).ToList();
            var actualOrdered = actualDevices.OrderBy(d => d.Name).ThenBy(d => d.Brand).ToList();

            // Assert
            Assert.Equal(expectedOrdered.Count, actualOrdered.Count);

            for (int i = 0; i < expectedOrdered.Count; i++)
            {
                //Assert.Equal(expectedOrdered[i].Id, actualOrdered[i].Id);
                Assert.Equal(expectedOrdered[i].Brand, actualOrdered[i].Brand);
                Assert.Equal(expectedOrdered[i].Name, actualOrdered[i].Name);
                Assert.Equal(expectedOrdered[i].Model, actualOrdered[i].Model);
                Assert.Equal(expectedOrdered[i].Colour, actualOrdered[i].Colour);
                Assert.Equal(expectedOrdered[i].Price, actualOrdered[i].Price);
            }
        }

        // Caso no hay dispositivos disponibles
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDeviceForPurchase_NotFound()
        {
            var emptyContext = CreateEmptyContext();
            var mockLogger = new Mock<ILogger<DevicesController>>();
            var controller = new DevicesController(emptyContext, mockLogger.Object);

            // Act
            var result = await controller.GetDevicesForPurchase(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDevices = Assert.IsType<List<DeviceForPurchaseDTO>>(okResult.Value);
            Assert.Empty(actualDevices);
        }

        private ApplicationDbContext CreateEmptyContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
