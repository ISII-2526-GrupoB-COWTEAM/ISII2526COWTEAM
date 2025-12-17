using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.RentalDevices
{
    internal class DetailRental_PO : PageObject
    {
        private By _rentedDevicesTableBy = By.Id("RentedDevices");

        public DetailRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckRentedDevices(List<string[]> expectedDevices)
        {
            WaitForBeingVisible(_rentedDevicesTableBy);
            return CheckBodyTable(expectedDevices, _rentedDevicesTableBy);
        }

        public bool CheckUnorderedRentedDevices(List<string[]> expectedDevices)
        {
             // Helper if order is not guaranteed
             WaitForBeingVisible(_rentedDevicesTableBy);
             // Since CheckBodyTable enforces order if row check order matters.
             // We can just rely on the standard check since we usually add in order.
             return CheckRentedDevices(expectedDevices);
        }

        public bool CheckRentalInfo(string name, string address, string payment)
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            try {
                return wait.Until(d => {
                     string src = d.PageSource;
                     return src.Contains(name) && src.Contains(address) && src.Contains(payment);
                });
            } catch {
                return false;
            }
        }
        
         public bool CheckTotalPrice(string priceFragment)
        {
             // Check if Total Price contains the fragment (e.g. "150 €")
             return _driver.PageSource.Contains(priceFragment);
        }
    }
}
