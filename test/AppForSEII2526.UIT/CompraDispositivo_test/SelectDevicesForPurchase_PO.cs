using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_CompraDispositivo
{
    public class SelectDevicesForPurchase_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputColor = By.Id("selectColor");
        By buttonSearchDevices = By.Id("searchDevices");
        By tableOfDevicesBy = By.Id("TableOfDevices");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonPurchaseDevices = By.Id("purchaseDeviceButton");

        public SelectDevicesForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        /*
        public void SearchDevice(string name, string color)
        {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);

            _driver.FindElement(inputColor).Clear();
            _driver.FindElement(inputColor).SendKeys(color);

            _driver.FindElement(buttonSearchDevices).Click();
        }
        */
        public void SearchDevice(string name, string color)
        {
            // Esperamos a que el input esté listo
            WaitForBeingVisible(inputName);

            // NAME
            var nameInput = _driver.FindElement(inputName);
            nameInput.Clear();
            if (!string.IsNullOrEmpty(name))
                nameInput.SendKeys(name);

            // COLOR
            var colorInput = _driver.FindElement(inputColor);
            colorInput.Clear();
            if (!string.IsNullOrEmpty(color))
                colorInput.SendKeys(color);

            // Buscar
            _driver.FindElement(buttonSearchDevices).Click();
        }

        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {

            return CheckBodyTable(expectedDevices, tableOfDevicesBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }


        public void AddDeviceToPurchaseCart(int deviceId)
        {
            WaitForBeingClickable(By.Id("deviceToPurchase_" + deviceId));
            _driver.FindElement(By.Id("deviceToPurchase_" + deviceId)).Click();
        }


        public void RemoveDeviceFromPurchaseCart(int deviceId)
        {
            WaitForBeingClickable(By.Id("removeDevice_" + deviceId));
            _driver.FindElement(By.Id("removeDevice_" + deviceId)).Click();
        }


        public bool PurchaseNotAvailable()
        {
            return _driver.FindElement(buttonPurchaseDevices).Displayed == false;
        }

        public bool NoDevicesAvailable()
        {
            WaitForBeingVisible(By.Id("TableOfDevices"));

            var rows = _driver.FindElement(By.Id("TableOfDevices"))
                              .FindElement(By.TagName("tbody"))
                              .FindElements(By.TagName("tr"));

            return rows.Count == 0;
        }






    }
}