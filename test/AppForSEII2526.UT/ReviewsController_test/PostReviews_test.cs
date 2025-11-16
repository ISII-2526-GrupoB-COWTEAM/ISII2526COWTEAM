using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.UT.ReviewsController_test
{
    public class PostReviews_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "Pablo";
        private const string _userCountry = "España";
        private const string _reviewTitle = "Reseña de prueba";


        public PostReviews_test()
        {
            var models = new List<Model>()
            {
                new Model("S14"),
                new Model("S15")
            };

            var devices = new List<Device>()
            {
                new Device("Samsung", "rojo", "muy bonito", 1, "Movil1", 100, 10, "media", models[0], 5, 5, 2024),
                new Device("Samsung", "azul", "muy nuevo", 2, "Movil2", 120, 12, "alta", models[1], 5, 5, 2025)
            };

            ApplicationUser user = new ApplicationUser(_userName, "Sanchez Martinez", _userCountry, "PaabloSM");

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReview()
        {
            var reviewNoItems = new ReviewForCreateDTO
            {
                ReviewTitle = _reviewTitle,
                ApplicationUserCountry = _userCountry,
                ReviewItems = new List<ReviewItemDTO>()
            };

            var reviewNoTitle = new ReviewForCreateDTO
            {
                ReviewTitle = "",
                ApplicationUserCountry = _userCountry,
                ReviewItems = new List<ReviewItemDTO>()
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 5, "Comentario") }
            };

            var reviewNoCountry = new ReviewForCreateDTO
            {
                ReviewTitle = _reviewTitle,
                ApplicationUserCountry = "",
                ReviewItems = new List<ReviewItemDTO>()
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 5, "Comentario") }
            };

            var reviewInvalidRating = new ReviewForCreateDTO
            {
                ReviewTitle = _reviewTitle,
                ApplicationUserCountry = _userCountry,
                ReviewItems = new List<ReviewItemDTO>()
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 6, "Comentario") }
            };

            var reviewDeviceNotExists = new ReviewForCreateDTO
            {
                ReviewTitle = _reviewTitle,
                ApplicationUserCountry = _userCountry,
                ReviewItems = new List<ReviewItemDTO>()
                { new ReviewItemDTO(99, "NoExiste", "XYZ", 2024, 5, "Comentario") }
            };

            var reviewNoComment = new ReviewForCreateDTO
            {
                ReviewTitle = _reviewTitle,
                ApplicationUserCountry = _userCountry,
                ReviewItems = new List<ReviewItemDTO>()
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 4, "") }
            };

            var allTests = new List<object[]>
            {
                new object[] { reviewNoItems, "Error! You must include at least one device to review" },
                new object[] { reviewNoTitle, "Error! The review title is required" },
                new object[] { reviewNoCountry, "Error! The country is required" },
                new object[] { reviewInvalidRating, "Error! Rating for device" },
                new object[] { reviewDeviceNotExists, "Error! Device named" },
                new object[] { reviewNoComment, "Error! You must include a comment for device" }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CreateReview))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReview_Error_test(ReviewForCreateDTO reviewDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;
            var controller = new ReviewsController(_context, logger);

            // Act
            var result = await controller.CreateReview(reviewDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReview_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;
            var controller = new ReviewsController(_context, logger);

            // DTO de entrada
            var reviewDTO = new ReviewForCreateDTO
            {
                ApplicationUserName = _userName,
                ApplicationUserCountry = _userCountry,
                ReviewTitle = _reviewTitle,
                ReviewItems = new List<ReviewItemDTO>() 
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 5, "Comentario") }
            };

            // DTO esperado
            var expectedReviewDetailDTO = new ReviewDetailDTO(
                0,                    
                _userName,            
                _userCountry,         
                _reviewTitle,         
                DateTime.Now,         
                new List<ReviewItemDTO>()
                { new ReviewItemDTO(1, "Movil1", "S14", 2024, 5, "Comentario") }
            );

            // Act
            var result = await controller.CreateReview(reviewDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualReviewDetailDTO = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            expectedReviewDetailDTO.Id = actualReviewDetailDTO.Id;
            expectedReviewDetailDTO.ReviewDate = actualReviewDetailDTO.ReviewDate;

            Assert.Equal(expectedReviewDetailDTO, actualReviewDetailDTO);
        }

    }
}
