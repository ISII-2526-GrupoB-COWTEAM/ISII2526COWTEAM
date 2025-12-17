using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_CompraDispositivo
{
    public class DetailPurchase_PO : PageObject
    {
        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckPurchaseDetail(string name, string surname, string deliveryAddress, string paymentMethod, DateTime purchaseDate, string totalPrice)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));

            bool result = true;

            var nameSurnameText = _driver.FindElement(By.Id("NameSurname")).Text;
            result = result && nameSurnameText.Contains(name);
            result = result && nameSurnameText.Contains(surname);

            result = result && _driver.FindElement(By.Id("DeliveryAddress")).Text.Contains(deliveryAddress);

            result = result && _driver.FindElement(By.Id("PaymentMethod")).Text.Contains(paymentMethod);

            result = result && _driver.FindElement(By.Id("TotalPrice")).Text.Contains(totalPrice);

            var actualPurchaseDate = DateTime.Parse(_driver.FindElement(By.Id("PurchaseDate")).Text);

            // margen de 1 minuto como en el ejemplo
            result = result && ((actualPurchaseDate - purchaseDate) < new TimeSpan(0, 1, 0));

            return result;
        }

        public bool CheckListOfPurchasedDevices(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, By.Id("PurchasedDevices"));
        }
    }
}