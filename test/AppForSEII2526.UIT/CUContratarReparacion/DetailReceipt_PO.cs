using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CUContratarReparacion
{
    public class DetailReceipt_PO : PageObject
    {
        private By _receiptItemsTableBy = By.Id("TableOfReceiptItems");

        public DetailReceipt_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReceiptItems(List<string[]> expectedItems)
        {
            WaitForBeingVisible(_receiptItemsTableBy);
            return CheckBodyTable(expectedItems, _receiptItemsTableBy);
        }

        public bool CheckClientInfo(string name, string surname, string address)
        {
            // Retry for a few seconds as text might load slightly after table structure
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            try {
                return wait.Until(d => 
                    d.PageSource.Contains(name) && 
                    d.PageSource.Contains(surname) && 
                    d.PageSource.Contains(address));
            } catch (WebDriverTimeoutException) {
                return false;
            }
        }
    }
}
