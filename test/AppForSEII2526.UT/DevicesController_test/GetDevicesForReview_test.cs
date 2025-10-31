using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
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
    }
}
