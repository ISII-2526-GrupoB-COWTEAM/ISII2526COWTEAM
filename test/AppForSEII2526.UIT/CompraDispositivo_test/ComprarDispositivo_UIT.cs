using AppForMovies.UIT.Shared;


namespace AppForSEII2526.UIT.CU_CompraDispositivo
{

    public class CUComprarDispositivo_UIT : UC_UIT
    {
        private SelectDevicesForPurchase_PO selectDevicesForPurchase_PO;
        private const int deviceId1 = 1;
        private const string deviceName1 = "iPhone 14";
        private const string deviceBrand1 = "Apple";
        private const string deviceModel1 = "iPhone 14";
        private const string deviceColor1 = "Black";
        private const string devicePrice1 = "999";

        private const int deviceId2 = 2;
        private const string deviceName2 = "Galaxy S23";
        private const string deviceBrand2 = "Samsung";
        private const string deviceModel2 = "Galaxy S23";
        private const string deviceColor2 = "White";
        private const string devicePrice2 = "899";




        public CUComprarDispositivo_UIT(ITestOutputHelper output) : base(output)
        {
            selectDevicesForPurchase_PO = new SelectDevicesForPurchase_PO(_driver, _output);
        }

/*
        private void Precondition_perform_login()
        {
            Perform_login("jaime.rodriguez7@alu.uclm.es", "ContraseñaPrueba");
        }
*/


        private void InitialStepsForPurchase()
        {
            Initial_step_opening_the_web_page();
          //  Precondition_perform_login();

            selectDevicesForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchase"));
            
            // Retry logic for StaleElementReferenceException
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _driver.FindElement(By.Id("CreatePurchase")).Click();
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    if (i == 2) throw; // Rethrow if last attempt fails
                    System.Threading.Thread.Sleep(500); // Small delay before retry
                }
            }
        }



        [Theory]
        [InlineData("jaime.rodriguez7@alu.uclm.es", "Rodriguez de Vera Martinez", "Avenida Castilla 12", "CreditCard")]
        [InlineData("jaime.rodriguez7@alu.uclm.es", "Rodriguez de Vera Martinez", "Avenida Castilla 12", "PayPal")]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_01_02_FlujoBasico(string name, string surname, string deliveryAddress, string paymentMethod)
        {

            var createPurchase = new CreatePurchase_PO(_driver, _output);
            var detailPurchase = new DetailPurchase_PO(_driver, _output);

            InitialStepsForPurchase();


            selectDevicesForPurchase_PO.AddDeviceToPurchaseCart(deviceName1);


            _driver.FindElement(By.Id("purchaseDevicesButton")).Click();


            createPurchase.FillInPurchaseInfo(
                name,
                surname,
                deliveryAddress,
                paymentMethod
            );


            createPurchase.PressPurchaseDevices();
            createPurchase.PressOkModalDialog();


            Assert.True(
                detailPurchase.CheckPurchaseDetail(name, surname, deliveryAddress, paymentMethod, DateTime.Now, devicePrice1 + " €"),
                "El detalle de la compra no es correcto"
            );


            var expectedDevices = new List<string[]>
            {
                new string[]
                {
                    deviceName1,   // Name
                    deviceBrand1,  // Brand
                    deviceColor1,  // Color
                    "2023"         // Year
                }
            };

            Assert.True(
                detailPurchase.CheckListOfPurchasedDevices(expectedDevices),
                "Los dispositivos comprados no son correctos"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_03_NoDispositivoDisponible()
        {

            InitialStepsForPurchase();

            selectDevicesForPurchase_PO.SearchDevice("NoExiste", "Rosa");


            Assert.True(
                selectDevicesForPurchase_PO.NoDevicesAvailable(),
                "No debería haber dispositivos disponibles para vender"
            );
        }



        [Theory]
        [InlineData(deviceName1, deviceBrand1, deviceModel1, deviceColor1, devicePrice1, "Pixel 8", "")]
        [InlineData(deviceName1, deviceBrand1, deviceModel1, deviceColor1, devicePrice1, "", "Azul")]
        [InlineData(deviceName1, deviceBrand1, deviceModel1, deviceColor1, devicePrice1, "Pixel 8", "Azul")]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_04_05_06_Filtros(string name, string brand, string model, string color, string price, string filterName, string filterColor)
        {
            InitialStepsForPurchase();
            var expectedDevices = new List<string[]> { new string[] { name, brand, model, color, price + " €" }, };

            selectDevicesForPurchase_PO.SearchDevice(filterName, filterColor);


            Assert.True(
                selectDevicesForPurchase_PO.CheckListOfDevices(expectedDevices),
                "La lista de dispositivos no coincide con el filtro aplicado"
            );
        }






        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_07_AñadirYQuitarDispositivo()
        {

            InitialStepsForPurchase();


            selectDevicesForPurchase_PO.AddDeviceToPurchaseCart(deviceName1);
            selectDevicesForPurchase_PO.RemoveDeviceFromPurchaseCart(deviceName1);


            Assert.True(
                selectDevicesForPurchase_PO.PurchaseNotAvailable(),
                "El botón de compra debe estar oculto si el carrito está vacío"
            );
        }



        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_08_ComprasinDispositivos()
        {

            InitialStepsForPurchase();


            Assert.True(
                selectDevicesForPurchase_PO.PurchaseNotAvailable(),
                "No se debe poder comprar sin dispositivos en el carrito"
            );
        }


        [Theory]
        [InlineData("", "Rodriguez de Vera Martinez", "Avenida Castilla 12", "CreditCard", "The CustomerName field is required.")]
        [InlineData("jaime.rodriguez7@alu.uclm.es", "", "Avenida Castilla 12", "CreditCard", "The CustomerSurname field is required.")]
        [InlineData("jaime.rodriguez7@alu.uclm.es", "Rodriguez de Vera Martinez", "", "CreditCard", "The DeliveryAddress field is required.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_9_10_11_12_DatosObligatorios(string name, string surname, string deliveryAddress, string paymentMethod, string expectedError)
        {

            InitialStepsForPurchase();
            var createPurchase = new CreatePurchase_PO(_driver, _output);

            selectDevicesForPurchase_PO.AddDeviceToPurchaseCart(deviceName1);
            _driver.FindElement(By.Id("purchaseDeviceButton")).Click();


            createPurchase.FillInPurchaseInfo(name, surname, deliveryAddress, paymentMethod);
            createPurchase.PressPurchaseDevices();


            Assert.True(
                createPurchase.CheckValidationError(expectedError),
                $"Debería aparecer el error: {expectedError}"
            );
        }



        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void CP_13_VolverAtrasTrasRellenarDatosUsuario()
        {

            InitialStepsForPurchase();
            var createPurchase = new CreatePurchase_PO(_driver, _output);


            selectDevicesForPurchase_PO.AddDeviceToPurchaseCart(deviceName1);
            _driver.FindElement(By.Id("purchaseDeviceButton")).Click();

            createPurchase.FillInPurchaseInfo(
                "jaime.rodriguez7@alu.uclm.es",
                "Rodriguez de Vera Martinez",
                "Av. Castilla 12",
                "CreditCard"
            );
    

            createPurchase.PressModifyDevices();


            _driver.FindElement(By.Id("purchaseDeviceButton")).Click();


            Assert.True(
                createPurchase.CheckPurchaseFormData(
                    "jaime.rodriguez7@alu.uclm.es",
                    "Rodriguez de Vera Martinez",
                    "Av. Castilla 12",
                    "CreditCard"
                ),
                "Los datos del usuario deberían mantenerse al volver al formulario"
            );
        }




    }
}