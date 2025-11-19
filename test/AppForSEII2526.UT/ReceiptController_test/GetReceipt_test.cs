using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReceiptDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ReceiptController_test
{
    public class GetReceipt_test : AppForSEII25264SqliteUT
    {
        public GetReceipt_test()
        {
            // Crear usuario
            var user = new ApplicationUser("Carlos", "Sanchez", "España", "CarlosS");

            // Crear escalas
            var scales = new List<Scale>
            {
                new Scale("Alta", 1),
                new Scale("Media", 2)
            };

            // Crear repairs
            var repairs = new List<Repair>
            {
                new Repair(120,"Cambio completo de pantalla OLED",1,"CambioPantalla",scales[0].Id, scales[0], new List<ReceiptItem>()),
                new Repair(50,"Reemplazo de la batería",2,"Bateria",scales[1].Id, scales[1], new List<ReceiptItem>())
            };

            // Crear receipt
            var receipt = new Receipt(
                Id: 1,
                ApplicationUser: user,
                ReceiptItem: new List<ReceiptItem>(),
                DeliveryAddress: "Calle Falsa 123",
                PaymentMethod: PaymentMethodTypes.Cash,
                ReceiptDate: DateTime.Today,
                TotalPrice: 170
            );

            // Crear receipt items
            var item1 = new ReceiptItem(repairs[0], receipt, "Modelo X");
            var item2 = new ReceiptItem(repairs[1], receipt, "Modelo Y");

            receipt.ReceiptItem.Add(item1);
            receipt.ReceiptItem.Add(item2);

            // Guardar todo en el contexto
            _context.Add(user);
            _context.AddRange(scales);
            _context.AddRange(repairs);
            _context.Add(receipt);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetReceipt_NotFound_test()
        {
            var logger = new Mock<ILogger<ReceiptController>>().Object;
            var controller = new ReceiptController(_context, logger);

            var result = await controller.GetReceiptDetail(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetReceipt_Found_test()
        {
            var logger = new Mock<ILogger<ReceiptController>>().Object;
            var controller = new ReceiptController(_context, logger);

            var result = await controller.GetReceiptDetail(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var receiptDTOActual = Assert.IsType<ReceiptDetailDTO>(okResult.Value);

            // DTO esperado
            var expectedReceipt = new ReceiptDetailDTO(
                1,
                "Carlos",
                "Sanchez",
                "Calle Falsa 123",
                receiptDTOActual.DateOfReceipt,
                170,
                new List<ReceiptItemDTO>
                {
                    new ReceiptItemDTO(1, "CambioPantalla", "Alta", 120, "Modelo X"),
                    new ReceiptItemDTO(2, "Bateria", "Media", 50, "Modelo Y")
                }
            );

            Assert.Equal(expectedReceipt, receiptDTOActual);
        }
    }
}
