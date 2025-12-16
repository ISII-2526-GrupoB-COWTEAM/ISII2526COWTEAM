using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.RentalController_test
{
    public class PostRental_test : AppForSEII25264SqliteUT
    {
        private const string _name = "Ana";
        private const string _surname = "Rodríguez de Vera";
        private const string _deliveryAddress = "Calle España";

        private const string _deviceName = "Iphone 11";
        private const string _deviceBrand = "Apple";
        private const string _deviceColor = "Lila";
        private const string _deviceModel = "Iphone 11";
        private const int _deviceYear = 2019;
        private const double _devicePriceForRent = 19.0;
        private const int _deviceQuantity = 1;

        public PostRental_test()
        {
            var models = new List<Model>
            {
                new Model(_deviceModel)
            };

            var devices = new List<Device>
            {
                new Device(
                    _deviceBrand,
                    _deviceColor,
                    "64GB",
                    5,                      
                    _deviceName,
                    194,                    
                    _devicePriceForRent,    
                    "Correcto",
                    models[0],
                    2,
                    2,
                    _deviceYear)
            };

            var user = new ApplicationUser(_name, _surname, "España", "anita24");

            
            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateRental()
        {
            var today = DateTime.Today;

            var devicesValid = new List<RentalDeviceDTO>
            {
                new RentalDeviceDTO(
                    1,                 
                    _deviceName,
                    _deviceBrand,
                    _deviceColor,
                    _deviceYear,
                    _deviceModel,
                    _devicePriceForRent,
                    _deviceQuantity)
            };

            var invalidAddress = new RentalForCreateDTO(
                _name, _surname,
                "Avenida de España",
                PaymentMethodTypes.CreditCard,
                today,
                today.AddDays(2),
                today.AddDays(5),
                new List<RentalDeviceDTO>());
                
            
            var rentalNoItem = new RentalForCreateDTO(
                _name, _surname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,                  
                today.AddDays(2),       
                today.AddDays(5),       
                new List<RentalDeviceDTO>());

            
            var rentalFromBeforeToday = new RentalForCreateDTO(
                _name, _surname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,                  
                today,                 
                today.AddDays(5),
                devicesValid);

            
            var rentalToBeforeFrom = new RentalForCreateDTO(
                _name, _surname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,                  
                today.AddDays(5),       
                today.AddDays(2),       
                devicesValid);

            
            var rentalUserNotFound = new RentalForCreateDTO(
                "NoExiste", "Nadie",
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,
                today.AddDays(2),
                today.AddDays(5),
                devicesValid);

            
            var rentalDeviceNotExists = new RentalForCreateDTO(
                _name, _surname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,
                today.AddDays(2),
                today.AddDays(5),
                new List<RentalDeviceDTO>
                {
                    new RentalDeviceDTO(
                        99,                 
                        "NoExiste",
                        "Marca",
                        "Color",
                        2024,
                        "Modelo",
                        10,
                        1)
                });

            var allTests = new List<object[]>
            {
                new object[] { rentalNoItem,          "Error! You must rent at least one device" },
                new object[] { rentalFromBeforeToday, "Error! Your rental date must start later than today" },
                new object[] { rentalToBeforeFrom,    "Error! The end date must be after the start date" },
                new object[] { rentalUserNotFound,    "Error! User not found" },
                new object[] { rentalDeviceNotExists, "Error! Device named" },
                new object[] { invalidAddress,        "Error! Por favor, introduce una dirección válida incluyendo las palabras Calle o Carretera" }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CreateRental))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<DevicesController>>();
            var controller = new RentalController(_context, mock.Object);

            var result = await controller.CreateRental(rentalDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Success_test()
        {
            var mock = new Mock<ILogger<DevicesController>>();
            var controller = new RentalController(_context, mock.Object);

            var today = DateTime.Today;
            var from = today.AddDays(2);
            var to = today.AddDays(5);

            var rentalDevices = new List<RentalDeviceDTO>
            {
                new RentalDeviceDTO(
                    1,
                    _deviceName,
                    _deviceBrand,
                    _deviceColor,
                    _deviceYear,
                    _deviceModel,
                    _devicePriceForRent,
                    _deviceQuantity)
            };

            var rentalDTO = new RentalForCreateDTO(
                _name, _surname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                today,      
                from,
                to,
                rentalDevices);

            
            var result = await controller.CreateRental(rentalDTO);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalForDetailDTO>(createdResult.Value);

            
            var expectedRentalDetailDTO = new RentalForDetailDTO(
                actualRentalDetailDTO.Id,              
                _name,
                _surname,
                _deliveryAddress,
                actualRentalDetailDTO.TotalPrice,      
                actualRentalDetailDTO.RentalDate,      
                actualRentalDetailDTO.RentalDateFrom,
                actualRentalDetailDTO.RentalDateTo,
                actualRentalDetailDTO.RentalDevices,   
                PaymentMethodTypes.CreditCard);

            Assert.Equal(expectedRentalDetailDTO, actualRentalDetailDTO);
        }
    }
}
