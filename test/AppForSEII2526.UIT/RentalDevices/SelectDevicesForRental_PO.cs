using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.RentalDevices
{
    internal class SelectDevicesForRental_PO : PageObject
    {
        private By _inputModelBy = By.Id("inputTitle"); // Note: ID in Razor is inputTitle
        private By _inputPriceBy = By.Id("inputGenre"); // Note: ID in Razor is inputGenre
        private By _searchButtonBy = By.Id("searchDevices");
        private By _devicesTableBy = By.Id("TableOfMovies"); // Note: ID in Razor is TableOfMovies
        private By _rentDevicesButtonBy = By.Id("purchaseDeviceButton"); // ID for Proceed button

        private IWebElement _inputModel() => _driver.FindElement(_inputModelBy);
        private IWebElement _inputPrice() => _driver.FindElement(_inputPriceBy);
        private IWebElement _searchButton() => _driver.FindElement(_searchButtonBy);
        private IWebElement _rentDevicesButton() => _driver.FindElement(_rentDevicesButtonBy);

        public SelectDevicesForRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FilterDevices(string? model, string? price)
        {
            WaitForBeingVisible(_devicesTableBy);
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(ElementNotInteractableException));

            if (!string.IsNullOrEmpty(model))
            {
                // Wait for visibility
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_inputModelBy));
                wait.Until(d => {
                     var el = _inputModel();
                     el.Clear();
                     el.SendKeys(model);
                     el.SendKeys(Keys.Tab);
                     return true;
                });
            }

            if (!string.IsNullOrEmpty(price))
            {
                 wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_inputPriceBy));
                 wait.Until(d => {
                     var el = _inputPrice();
                     el.Clear();
                     el.SendKeys(price);
                     el.SendKeys(Keys.Tab);
                     return true;
                });
            }

            WaitForBeingVisible(_searchButtonBy);
            // JS Click safe
            JSClick(_searchButton());
            System.Threading.Thread.Sleep(500); 
        }

        public bool CheckDevicesList(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, _devicesTableBy);
        }

        private void JSClick(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        public void SelectDevices(List<string> deviceNames)
        {
            foreach (var name in deviceNames)
            {
                var safeName = name.Replace(" ", "_");
                var buttonId = By.Id($"deviceToRent_{safeName}");
                
                // Robustness: Scroll and Wait for Clickable
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(buttonId));
                var btn = _driver.FindElement(buttonId);
                
                // Scroll into view
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                System.Threading.Thread.Sleep(200); // Small pause after scroll

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(btn));
                
                JSClick(btn);
                
                // CRITICAL: Wait for the item to appear in the cart to ensure Add succeeded
                var removeButtonId = By.Id($"removeDevice_{safeName}");
                WaitForBeingVisible(removeButtonId);
                System.Threading.Thread.Sleep(500); // Wait for potential UI updates when adding multiple items
            }
        }

        public void ProceedToRent()
        {
            WaitForBeingVisible(_rentDevicesButtonBy);
            // Use JS Click
            JSClick(_rentDevicesButton());
        }

        public bool IsRentButtonEnabled()
        {
             try {
                // If Hidden, it might not be interactable or "Enabled" check might fail or it might satisfy "Displayed == false".
                // In Razor: hidden="@hideRentingCart". If hidden, FindElement might fail or Displayed is false
                var btn = _driver.FindElements(_rentDevicesButtonBy);
                if (btn.Count == 0) return false;
                return btn[0].Displayed && btn[0].Enabled;
            } catch (Exception) {
                return false;
            }
        }

        public void RemoveDeviceFromCart(string deviceName)
        {
             var safeName = deviceName.Replace(" ", "_");
             var removeButton = By.Id($"removeDevice_{safeName}");
             WaitForBeingVisible(removeButton);
             JSClick(_driver.FindElement(removeButton));
             System.Threading.Thread.Sleep(200);
        }
        
        public bool CheckMessage(string message)
        {
            return _driver.PageSource.Contains(message);
        }
    }
}
