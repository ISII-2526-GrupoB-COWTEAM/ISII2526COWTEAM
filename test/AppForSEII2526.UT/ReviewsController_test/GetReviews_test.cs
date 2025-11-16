using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewsController_test
{
    public class GetReview_test : AppForSEII25264SqliteUT
    {
        public GetReview_test()
        {
            var models = new List<Model>()
            {
                new Model("S14"),
                new Model("S15")
            };

            var devices = new List<Device>()
            {
                new Device("Samsung", "rojo", "muy bonito", 1, "Movil1", 100, 10, "media", models[0], 5, 5, 2024),
                new Device("Apple", "verde", "muy caro", 2, "Movil2", 150, 15, "alta", models[1], 5, 5, 2025)
            };

            var user = new ApplicationUser("Pablo", "Sanchez Martinez", "España", "PaabloSM");

            var review = new Review(user, DateTime.Today, "Reseña sobre S14", new List<ReviewItem>());
            var reviewItem = new ReviewItem(devices[0], review, "Funciona muy bien", 4);
            review.ReviewItems.Add(reviewItem);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.Add(review);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;
            var controller = new ReviewsController(_context, logger);

            // Act
            var result = await controller.GetReview(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;
            var controller = new ReviewsController(_context, logger);

            // Act
            var result = await controller.GetReview(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);

            var expectedReview = new ReviewDetailDTO(
                reviewDTOActual.Id,                    
                reviewDTOActual.ApplicationUserName,   
                reviewDTOActual.ApplicationUserCountry,
                reviewDTOActual.ReviewTitle,           
                reviewDTOActual.ReviewDate,            
                new List<ReviewItemDTO>()
            );

            expectedReview.ReviewItems.Add(new ReviewItemDTO(
                1,
                "Movil1",
                "S14",
                2024,
                4,
                "Funciona muy bien"
            ));

            Assert.Equal(expectedReview, reviewDTOActual);
        }

    }
}
