using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CUHacerReseñas
{
    public class SelectDevicesForReview_PO : PageObject
    {
        private By _inputBrandBy = By.Id("inputBrand");
        private By _inputYearBy = By.Id("inputYear");
        private By _searchButtonBy = By.Id("searchDevices");
        private By _devicesTableBy = By.Id("TableOfDevices");
        private By _reviewDevicesButtonBy = By.Id("reviewDevicesButton");

        private IWebElement _inputBrand() => _driver.FindElement(_inputBrandBy);
        private IWebElement _inputYear() => _driver.FindElement(_inputYearBy);
        private IWebElement _searchButton() => _driver.FindElement(_searchButtonBy);
        private IWebElement _reviewDevicesButton() => _driver.FindElement(_reviewDevicesButtonBy);

        public SelectDevicesForReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FilterDevices(string? brand, string? year)
        {
            WaitForBeingVisible(_devicesTableBy);

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            if (!string.IsNullOrEmpty(brand))
            {
                wait.Until(d => {
                    var el = _inputBrand();
                    el.Clear();
                    el.SendKeys(brand);
                    return true;
                });
            }

            if (!string.IsNullOrEmpty(year))
            {
                wait.Until(d => {
                    var el = _inputYear();
                    el.Clear();
                    el.SendKeys(year);
                    return true;
                });
            }

            WaitForBeingVisible(_searchButtonBy);
            _searchButton().Click();
            System.Threading.Thread.Sleep(500); 
        }

        public bool CheckDevicesList(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, _devicesTableBy);
        }

        public void SelectDevices(List<string> deviceNames)
        {
            foreach (var deviceName in deviceNames)
            {
                // Retry logic for StaleElementReferenceException
                var attempts = 0;
                while (attempts < 3)
                {
                    try
                    {
                        var xpathBy = By.XPath($"//tr[td[normalize-space() = '{deviceName}']]//button[starts-with(@id, 'deviceToReview_')]");
                        WaitForBeingVisible(xpathBy);
                        var button = _driver.FindElement(xpathBy);
                        button.Click();
                        System.Threading.Thread.Sleep(1000); // Wait for server to process
                        break; // Success
                    }
                    catch (StaleElementReferenceException)
                    {
                        attempts++;
                        System.Threading.Thread.Sleep(500); 
                    }
                     catch (ElementClickInterceptedException)
                    {
                         attempts++;
                         System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }

        public void ProceedToReview()
        {
             // Retry logic for Proceed button in case it appears late
            var attempts = 0;
            while(attempts < 3)
            {
                try
                {
                    WaitForBeingVisible(_reviewDevicesButtonBy);
                    _reviewDevicesButton().Click();
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++; 
                     System.Threading.Thread.Sleep(500); 
                }
                 catch (WebDriverTimeoutException)
                {
                    // If validation failed or cart empty, it won't be visible.
                     // But we want to fail if it's not visible when expected. 
                     // Rewrap or just let it throw on last attempt?
                     if (attempts == 2) throw;
                     attempts++;
                }
            }
        }

        public bool IsReviewButtonEnabled()
        {
            try {
                WaitForBeingVisible(_reviewDevicesButtonBy);
                return _reviewDevicesButton().Enabled;
            } catch (Exception) {
                return false;
            }
        }

        public void RemoveDeviceFromReview(string deviceName)
        {
            // ID format: removeDevice_Name. If Name has spaces: removeDevice_Dell XPS 15
            // Use css matching or xpath to avoid space issues in basic selectors if any
            var xpathString = $"//button[starts-with(@id, 'removeDevice_') and contains(@id, '{deviceName}')]";
            var xpathBy = By.XPath(xpathString);
            
            WaitForBeingVisible(xpathBy);
            _driver.FindElement(xpathBy).Click();
            System.Threading.Thread.Sleep(1000); // Increased wait
        }
    }
}
