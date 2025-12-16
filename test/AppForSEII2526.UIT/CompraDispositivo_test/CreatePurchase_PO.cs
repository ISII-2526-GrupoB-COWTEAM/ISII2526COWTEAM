using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_CompraDispositivo
{
    public class CreatePurchase_PO : PageObject
    {
        private By _nameBy = By.Id("Name");
        private By _surnameBy = By.Id("Surname");

        private IWebElement _name() => _driver.FindElement(_nameBy);
        private IWebElement _surname() => _driver.FindElement(_surnameBy);
        private IWebElement _deliveryAddress() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement _paymentMethod() => _driver.FindElement(By.Id("PaymentMethod"));

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillInPurchaseInfo(string name, string surname, string deliveryAddress, string paymentMethod)
        {
            WaitForBeingVisible(_nameBy);
            WaitForBeingVisible(_surnameBy);

            _name().SendKeys(name);
            _surname().SendKeys(surname);
            _deliveryAddress().SendKeys(deliveryAddress);

            SelectElement selectElement = new SelectElement(_paymentMethod());
            selectElement.SelectByValue(paymentMethod);

        }

        public void PressPurchaseDevices()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyDevices()
        {
            _driver.FindElement(By.Id("ModifyDevices")).Click();
        }


        public bool CheckListOfPurchaseItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, By.Id("TableOfPurchaseItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }


        public bool CheckPurchaseFormData(string name, string surname, string deliveryAddress, string paymentMethod)
        {
            bool result = true;

            result &= _driver.FindElement(By.Id("Name"))
                             .GetAttribute("value")
                             .Contains(name);

            result &= _driver.FindElement(By.Id("Surname"))
                             .GetAttribute("value")
                             .Contains(surname);

            result &= _driver.FindElement(By.Id("DeliveryAddress"))
                             .GetAttribute("value")
                             .Contains(deliveryAddress);

            result &= _driver.FindElement(By.Id("PaymentMethod"))
                             .GetAttribute("value")
                             .Contains(paymentMethod);

            return result;
        }

    }
}