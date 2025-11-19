using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RepairDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.RepairController_test
{
    public class GetRepairs_test : AppForSEII25264SqliteUT
    {
        public GetRepairs_test()
        {
            // Crear scales válidos según tu modelo
            var scales = new List<Scale>()
            {
                new Scale("Alta", 1),
                new Scale("Media", 2)
            };


            // Crear repairs válidos usando tu constructor real
            var repairs = new List<Repair>()
            {
                new Repair(120,"Cambio completo de pantalla OLED",1,"CambioPantalla",scales[0].Id, scales[0], new List<ReceiptItem>()),
                new Repair(50,"Reemplazo de la batería",2,"Bateria",scales[1].Id, scales[1],new List<ReceiptItem>())
            };

           

            _context.AddRange(scales);
            _context.AddRange(repairs);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetRepair_OK()
        {
            // Lista de DTOs esperados
            var repairDTOs = new List<RepairDTO>()
            {
                new RepairDTO(
                    id: 1,
                    name: "CambioPantalla",
                    scale: "Alta",
                    cost: 120,
                    description: "Cambio completo de pantalla OLED"
                ),
                new RepairDTO(
                    id: 2,
                    name: "Bateria",
                    scale: "Media",
                    cost: 50,
                    description: "Reemplazo de la batería"
                )
            };

            // Casos de prueba
            // 1. Sin filtros → devuelve todos
            var tc1 = new List<RepairDTO>()
            {
                repairDTOs[0], repairDTOs[1]
            }.OrderBy(r => r.Id).ToList();

            // 2. Filtrar por Scale = "Alta"
            var tc2 = new List<RepairDTO>()
            {
                repairDTOs[0]
            };

            // 3. Filtrar por Scale = "Media"
            var tc3 = new List<RepairDTO>()
            {
                repairDTOs[1]
            };

            // 4. Filtrar por Name = "Bateria"
            var tc4 = new List<RepairDTO>()
            {
                repairDTOs[1]
            };

            // Ajusta los parámetros según cómo sea tu endpoint
            var allTests = new List<object[]>
            {
                // filtros: scale, name
                new object[] { null, null, tc1 },
                new object[] { "Alta", null, tc2 },
                new object[] { "Media", null, tc3 },
                new object[] { null, "Bateria", tc4 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetRepair_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRepairs_OK_test(string? filtroScale, string? filtroName, IList<RepairDTO> expectedRepairs)
        {
            // Arrange
            var controller = new RepairController(_context, null);

            // ⚠️ CORRECTO: el controller recibe (NombreReparacion, NombreEscala)
            var result = await controller.GetRepairs(filtroName, filtroScale);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var repairDTOsActual = Assert.IsType<List<RepairDTO>>(okResult.Value);
            Assert.Equal(expectedRepairs, repairDTOsActual);
        }



    }
}
