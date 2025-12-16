using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CUContratarReparacion
{
    public class SelectRepairsForReceipt_PO : PageObject
    {
        private By _inputRepairNameBy = By.Id("inputRepairName");
        private By _inputScaleBy = By.Id("inputScale");
        private By _searchButtonBy = By.Id("searchRepairs");
        private By _reapirsTableBy = By.Id("TableOfRepairs");
        private By _contractRepairsButtonBy = By.Id("contractRepairsButton");

        private IWebElement _inputRepairName() => _driver.FindElement(_inputRepairNameBy);
        private IWebElement _inputScale() => _driver.FindElement(_inputScaleBy);
        private IWebElement _searchButton() => _driver.FindElement(_searchButtonBy);
        private IWebElement _contractRepairsButton() => _driver.FindElement(_contractRepairsButtonBy);

        public SelectRepairsForReceipt_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FilterRepairs(string? name, string? scale)
        {
            // Wait for initial data load to complete to avoid StaleElementReferenceException
            // The table appears once OnInitializedAsync completes
            WaitForBeingVisible(_reapirsTableBy);

            // Retry mechanism for robustness against Blazor re-renders
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            if (!string.IsNullOrEmpty(name))
            {
                wait.Until(d => {
                    var el = _inputRepairName();
                    el.Clear();
                    el.SendKeys(name);
                    return true;
                });
            }

            if (!string.IsNullOrEmpty(scale))
            {
                wait.Until(d => {
                    var el = _inputScale();
                    el.Clear();
                    el.SendKeys(scale);
                    return true;
                });
            }

            WaitForBeingVisible(_searchButtonBy);
            _searchButton().Click();
            // Wait for table to reload - usually handled by wait for visibility of results or explicit wait
            System.Threading.Thread.Sleep(500); 
        }

        public bool CheckRepairsList(List<string[]> expectedRepairs)
        {
            return CheckBodyTable(expectedRepairs, _reapirsTableBy);
        }

        public void SelectRepairs(List<string> repairNames)
        {
            foreach (var repairName in repairNames)
            {
                // Button ID format: repairToReceipt_RepairName
                var buttonId = By.Id($"repairToReceipt_{repairName}");
                WaitForBeingVisible(buttonId);
                _driver.FindElement(buttonId).Click();
                System.Threading.Thread.Sleep(200); // Small wait for cart update
            }
        }

        public void ProceedToContract()
        {
            WaitForBeingVisible(_contractRepairsButtonBy);
            _contractRepairsButton().Click();
        }

        public bool IsContractButtonEnabled()
        {
             // Wait briefly to ensure UI state is settled
            try {
                WaitForBeingVisible(_contractRepairsButtonBy);
                return _contractRepairsButton().Enabled;
            } catch (Exception) {
                return false;
            }
        }

        public void RemoveRepairFromCart(string repairName)
        {
            // ID format: removeRepair_RepairName
            var removeButton = By.Id($"removeRepair_{repairName}");
            WaitForBeingVisible(removeButton);
            _driver.FindElement(removeButton).Click();
            System.Threading.Thread.Sleep(200); // Wait for update
        }
    }
}
