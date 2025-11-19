using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.RentalController_test
{
    public class GetRental_test : AppForSEII25264SqliteUT
    {
        public GetRental_test()
        {
            var models = new List<Model>
            {
                new Model("Iphone 11")
            };

            var devices = new List<Device>
            {
                new Device(
                    "Apple",
                    "Lila",
                    "64GB",
                    5,
                    "Iphone 11",
                    194,
                    19,
                    "Correcto",
                    models[0],
                    2,
                    2,
                    2019)
            };

            var user = new ApplicationUser(
                "Ana",
                "Rodríguez de Vera",
                "España",
                "anita24");

          
            var rental = new Rental(
                "Avenida de España",
                PaymentMethodTypes.CreditCard,
                user,
                new DateTime(2020, 12, 20), 
                new DateTime(2020, 12, 20), 
                new DateTime(2020, 12, 22), 
                new List<RentDevice>());



            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.Add(rental);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            var mock = new Mock<ILogger<DevicesController>>();
            var controller = new RentalController(_context, mock.Object);

            var result = await controller.GetRental(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_Found_test()
        {
            var mock = new Mock<ILogger<DevicesController>>();
            var controller = new RentalController(_context, mock.Object);

            var result = await controller.GetRental(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalForDetailDTO>(okResult.Value);

            
            var expectedRental = new RentalForDetailDTO(
                rentalDTOActual.Id,
                "Ana",
                "Rodríguez de Vera",
                "Avenida de España",
                rentalDTOActual.TotalPrice,
                rentalDTOActual.RentalDate,
                rentalDTOActual.RentalDateFrom,
                rentalDTOActual.RentalDateTo,
                rentalDTOActual.RentalDevices,
                PaymentMethodTypes.CreditCard
            );

            Assert.Equal(expectedRental, rentalDTOActual);
        }
    }
}