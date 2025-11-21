using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReceiptDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.OpenApi.Writers;

namespace AppForSEII2526.UT.ReceiptController_test
{
    public class PostReceipt_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "Carlos";
        private const string _userSurname = "Sanchez";
        private const string _deliveryAddress = "Calle Falsa 123";

        private const string _deliveryAddress2 = "Falsa 123";

        public PostReceipt_test()
        {
            // Crear usuario
            ApplicationUser user = new ApplicationUser(_userName, _userSurname, "España", "CarlosS");

            // Crear escalas
            var scales = new List<Scale>()
            {
                new Scale("1:18", 1),
                new Scale("1:24", 2)
            };


            // Crear reparaciones
            var repairs = new List<Repair>()
            {
                new Repair(120.5f, "Cambio ruedas", 1, "Cambio ruedas", 1, scales[0], new List<ReceiptItem>()),
                new Repair(80f, "Pintura", 2, "Pintura coche", 2, scales[1], new List<ReceiptItem>())
            };

            _context.Add(user);
            _context.AddRange(scales);
            _context.AddRange(repairs);
            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_CreateReceipt()
        {
            // Sin items
            var noItems = new ReceiptForCreate(
                "Carlos",
                "Sanchez",
                "Calle Falsa 123",
                new List<ReceiptItemDTO>()
            );

            // Sin nombre
            var noName = new ReceiptForCreate(
                "",
                "Sanchez",
                "Calle Falsa 123",
                new List<ReceiptItemDTO>()
                { new ReceiptItemDTO(1, "Cambio ruedas", "1:18", 120.5, "Ferrari F40") }
            );

            // Sin apellido
            var noSurname = new ReceiptForCreate(
                "Carlos",
                "",
                "Calle Falsa 123",
                new List<ReceiptItemDTO>()
                { new ReceiptItemDTO(1, "Cambio ruedas", "1:18", 120.5, "Ferrari F40") }
            );

            // Sin dirección
            var noAddress = new ReceiptForCreate(
                "Carlos",
                "Sanchez",
                "",
                new List<ReceiptItemDTO>()
                { new ReceiptItemDTO(1, "Cambio ruedas", "1:18", 120.5, "Ferrari F40") }
            );

            //Dirección sin calle o avenida
            var sprint2Adress = new ReceiptForCreate(
                "Carlos",
                "Sanchez",
                "Falsa 123",
                new List<ReceiptItemDTO>()
                { new ReceiptItemDTO(1, "Cambio ruedas", "1:18", 120.5, "Ferrari F40") }
            );

            // Reparación inexistente
            var repairNotExists = new ReceiptForCreate(
                "Carlos",
                "Sanchez",
                "Calle Falsa 123",
                new List<ReceiptItemDTO>()
                { new ReceiptItemDTO(99, "No existe", "1:18", 150, "Modelo X") }
            );

            return new List<object[]>
            {
                new object[] { noItems, "Error! You must include at least one repair" },
                new object[] { noName, "Error! The user's name is required" },
                new object[] { noSurname, "Error! The user's surname is required" },
                new object[] { noAddress, "Error! The delivery address is required" },
                new object[] { repairNotExists, "Error! Repair with ID" },
                new object[] { sprint2Adress, "Error en la dirección de envío. Por favor, introduce una dirección válida incluyendo las palabras Calle o Avenida" }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CreateReceipt))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReceipt_Error_test(ReceiptForCreate dto, string expectedError)
        {
            // Arrange
            var mock = new Mock<ILogger<ReceiptController>>();
            ILogger<ReceiptController> logger = mock.Object;
            var controller = new ReceiptController(_context, logger);

            // Act
            var result = await controller.CreateReceipt(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            var actualError = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(expectedError, actualError);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReceipt_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReceiptController>>();
            ILogger<ReceiptController> logger = mock.Object;
            var controller = new ReceiptController(_context, logger);

            var dto = new ReceiptForCreate(
                _userName,
                _userSurname,
                _deliveryAddress,
                new List<ReceiptItemDTO>()
                {
                    new ReceiptItemDTO(1, "Cambio ruedas", "1:18", 120.5, "Ferrari F40")
                }
            );

            // Act
            var result = await controller.CreateReceipt(dto);

            // Assert
            var created = Assert.IsType<CreatedResult>(result);
            var receipt = Assert.IsType<ReceiptDetailDTO>(created.Value);

            Assert.Equal(_userName, receipt.ApplicationUserName);
            Assert.Equal(_userSurname, receipt.ApplicationUserSurname);
            Assert.Equal(_deliveryAddress, receipt.DeliveryAddress);
            Assert.Single(receipt.ReceiptItems);
            Assert.Equal(120.5, receipt.TotalPrice);
        }
    }
}