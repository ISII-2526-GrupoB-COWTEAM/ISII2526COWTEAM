using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.DeviceDTO;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.DevicesController_test
{
    public class GetDevicesForReview_test : AppForSEII25264SqliteUT
    {
        public GetDevicesForReview_test()
        {
            //Definir datos de las pruebas reutilizados entre varias pruebas
            var models = new List<Model>() {
                new Model("S14"), 
                new Model("S15"), 
                new Model("Pro15"), 
                new Model("Pro16"),
            };
            
            var devices = new List<Device>() {
                new Device("Samsung", "rojo", "muy bonito", 1, "Movil1", 100, 10, "media", models[0],5,5,2024),
                new Device("Samsung", "azul", "muy nuevo", 2, "Movil2", 120, 12, "alta", models[1],5,5,2025),
                new Device("Apple", "amarillo", "muy feo", 3, "Movil3", 90, 9, "media", models[2],5,5,2024),
                new Device("Apple", "verde", "muy caro", 4, "Movil4", 150, 15, "muy alta", models[3],5,5,2025),

            };

            ApplicationUser user = new ApplicationUser("Pablo", "Sanchez Martinez", "España", "PaabloSM");

            var review = new Review(user, new DateTime(2025, 10, 12), "sobre s14", new List<ReviewItem>());

            var reviewItem = new ReviewItem(devices[0], review, "funciona muy bien", 4);
            review.ReviewItems.Add(reviewItem);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.Add(review);
            _context.SaveChanges();


        }

        public static IEnumerable<object[]> TestCasesFor_GetDevicesForReview_OK()
        {
            // Lista completa de los DTOs esperados
            var deviceDTOs = new List<DeviceForReviewDTO>()
            {
                new DeviceForReviewDTO(1, "Movil1", "Samsung", "rojo", 2024, "S14"),
                new DeviceForReviewDTO(2, "Movil2", "Samsung", "azul", 2025, "S15"),
                new DeviceForReviewDTO(3, "Movil3", "Apple", "amarillo", 2024, "Pro15"),
                new DeviceForReviewDTO(4, "Movil4", "Apple", "verde", 2025, "Pro16"),
            };

            // Casos de prueba
            // 1️. Sin filtros: devuelve todos
            var tc1 = new List<DeviceForReviewDTO>()
            {
                deviceDTOs[0], deviceDTOs[1], deviceDTOs[2], deviceDTOs[3]
            }.OrderBy(d => d.Id).ToList();

            // 2️. Filtrar por brand = Samsung
            var tc2 = new List<DeviceForReviewDTO>()
            {
                deviceDTOs[0], deviceDTOs[1]
            }.OrderBy(d => d.Id).ToList();

            // 3️. Filtrar por year = 2024
            var tc3 = new List<DeviceForReviewDTO>()
            {
                deviceDTOs[0], deviceDTOs[2]
            }.OrderBy(d => d.Id).ToList();

            // 4️. Filtrar por brand + year = Apple 2025
            var tc4 = new List<DeviceForReviewDTO>()
            {
                deviceDTOs[3]
            };

            var allTests = new List<object[]>
            {
                new object[] { null, null, tc1 },
                new object[] { "Samsung", null, tc2 },
                new object[] { null, 2024, tc3 },
                new object[] { "Apple", 2025, tc4 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDevicesForReview_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDevicesForReview_OK_test(string? filtroBrand, int? filtroYear,
            IList<DeviceForReviewDTO> expectedDevices)
        {
            // Arrange
            var controller = new DevicesController(_context, null);

            // Act
            var result = await controller.GetDevicesForReview(filtroBrand, filtroYear);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deviceDTOsActual = Assert.IsType<List<DeviceForReviewDTO>>(okResult.Value);
            Assert.Equal(expectedDevices, deviceDTOsActual);
        }

    }
}
