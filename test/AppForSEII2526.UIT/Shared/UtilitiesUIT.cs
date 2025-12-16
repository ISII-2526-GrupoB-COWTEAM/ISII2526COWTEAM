using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Namespace adapted to the current project
namespace AppForSEII2526.UIT.Shared
{
    public static class UtilitiesUIT
    {
        // Change this URL to matches your running Web application port
        private static string _URI = "http://localhost:5063/";

        public static void SetUp_UIT(out IWebDriver driver, out string URI)
        {
            URI = _URI;
            
            // Driver selection - defaulting to Chrome or Edge. 
            // You can change this behavior or read from config if needed.
            // Using logic similar to UC_UIT but simplified for this static helper.

            // Using Edge as seen in UC_UIT, or Chrome. 
            // Let's use Edge matching the existing file preference, but make it robust.
            var optionsEdge = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            // optionsEdge.AddArgument("--headless"); // Uncomment to run headless

            driver = new EdgeDriver(optionsEdge);
            
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        public static void WaitForBeingVisible(IWebDriver driver, By by)
        {
             var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
             wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

    }
}
