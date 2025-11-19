using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeviceDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.DevicesController_test
{
    public class GetDevicesForRental_test : AppForSEII25264SqliteUT
    {
        public GetDevicesForRental_test()
        {
            var models = new List<Model>()
            {
                new Model("Iphone 11"),
                new Model("Samsung S"),
                new Model("Huawei P30"),
                new Model("Iphone 17"),
            };

            var devices = new List<Device>()
            {
                new Device("Apple",  "Lila",  "64GB",   5, "Iphone 11", 194,  19,  "Correcto", models[0], 2, 2, 2019),
                new Device("Samsung","Azul",  "32GB",   6, "Samsung S",122,  12,  "Correcto", models[1], 3, 2, 2020),
                new Device("Huawei", "Azul",  "128GB",  8, "Huawei P30",136, 13,  "Muy bueno",models[2], 2, 5, 2023),
                new Device("Apple",  "Plata", "256GB",  9, "Iphone 17",1320,132, "Premium",  models[3], 1, 1, 2025),
            };

            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDevicesForRental_OK()
        {
            var deviceDTOs = new List<DeviceForRentalDTO>()
            {
                new DeviceForRentalDTO(5, "Iphone 11",  "Apple",  "Lila",  2019, "Iphone 11", 19),
                new DeviceForRentalDTO(6, "Samsung S",  "Samsung","Azul",  2020, "Samsung S", 12),
                new DeviceForRentalDTO(8, "Huawei P30", "Huawei", "Azul",  2023, "Huawei P30",13),
                new DeviceForRentalDTO(9, "Iphone 17",  "Apple",  "Plata", 2025, "Iphone 17", 132),
            };

            var tc1 = new List<DeviceForRentalDTO>()
            {
                deviceDTOs[0], deviceDTOs[1], deviceDTOs[2], deviceDTOs[3]
            }.OrderBy(d => d.Id).ToList();

            var tc2 = new List<DeviceForRentalDTO>()
            {
                deviceDTOs[0], deviceDTOs[3]
            }.OrderBy(d => d.Id).ToList();

            var tc3 = new List<DeviceForRentalDTO>()
            {
                deviceDTOs[0], deviceDTOs[1], deviceDTOs[2]
            }.OrderBy(d => d.Id).ToList();

            var tc4 = new List<DeviceForRentalDTO>()
            {
                deviceDTOs[0]
            };

            var allTests = new List<object[]>
            {
                new object[] { null,     null,  tc1 },     
                new object[] { "Apple",  null,  tc2 },     
                new object[] { null,     20.0,  tc3 },     
                new object[] { "Apple",  100.0, tc4 },     
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDevicesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDevicesForRental_OK_test(string? filtroModel, double? filtroPrice,
            IList<DeviceForRentalDTO> expectedDevices)
        {
            var controller = new DevicesController(_context, null);

            var result = await controller.GetDevicesForRental(filtroModel, filtroPrice);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var deviceDTOsActual = Assert.IsType<List<DeviceForRentalDTO>>(okResult.Value);
            Assert.Equal(expectedDevices, deviceDTOsActual);
        }
    }
}
