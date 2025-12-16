using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CUContratarReparacion
{
    public class CreateRepair_PO : PageObject
    {
        private By _nameInputBy = By.Id("Name");
        private By _surnameInputBy = By.Id("Surname");
        private By _addressInputBy = By.Id("DeliveryAddress");
        private By _paymentSelectBy = By.Id("PaymentMethod");
        private By _submitButtonBy = By.Id("Submit");
        private By _modifyRepairsButtonBy = By.Id("ModifyRepairs");
        private By _errorsShownBy = By.Id("ErrorsShown");
        // Dialog check
        private By _dialogOkButtonBy = By.Id("Button_DialogOK");

        private IWebElement _nameInput() => _driver.FindElement(_nameInputBy);
        private IWebElement _surnameInput() => _driver.FindElement(_surnameInputBy);
        private IWebElement _addressInput() => _driver.FindElement(_addressInputBy);
        private IWebElement _paymentSelect() => _driver.FindElement(_paymentSelectBy);
        private IWebElement _submitButton() => _driver.FindElement(_submitButtonBy);
        private IWebElement _modifyRepairsButton() => _driver.FindElement(_modifyRepairsButtonBy);


        public CreateRepair_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SetName(string name)
        {
            WaitForBeingVisible(_nameInputBy);
            _nameInput().Clear();
            _nameInput().SendKeys(name);
        }

        public void SetSurname(string surname)
        {
            WaitForBeingVisible(_surnameInputBy);
            _surnameInput().Clear();
            _surnameInput().SendKeys(surname);
        }

        public void SetAddress(string address)
        {
            WaitForBeingVisible(_addressInputBy);
            _addressInput().Clear();
            _addressInput().SendKeys(address);
        }

        public void SetPaymentMethod(string paymentMethod)
        {
            WaitForBeingVisible(_paymentSelectBy);
            var selectElement = new SelectElement(_paymentSelect());
            selectElement.SelectByText(paymentMethod);
        }

        public void SetModelForRepair(int repairId, string model)
        {
            var modelInputBy = By.Id($"model_{repairId}");
            WaitForBeingVisible(modelInputBy);
            var input = _driver.FindElement(modelInputBy);
            input.Clear();
            input.SendKeys(model);
        }

        // Helper for when RepairID is unknown (using row order)
        public void SetModelByIndex(int index, string model)
        {
            // Find inputs that start with "model_"
            // Since we don't have easily predictable IDs without DB knowledge, we grab all matching inputs
            var inputs = _driver.FindElements(By.XPath("//input[starts-with(@id, 'model_')]"));
            if (index < inputs.Count)
            {
                inputs[index].Clear();
                inputs[index].SendKeys(model);
            }
            else
            {
                throw new NoSuchElementException($"Model input at index {index} not found. Count: {inputs.Count}");
            }
        }

        public void SubmitContract()
        {
            WaitForBeingVisible(_submitButtonBy);
            _submitButton().Click();
            
            // Handle Dialog if present (Razor has a Dialog component)
             try
            {
                // Wait briefly for dialog
                WaitForBeingVisible(_dialogOkButtonBy);
                _driver.FindElement(_dialogOkButtonBy).Click();
            }
            catch (WebDriverTimeoutException)
            {
                // Dialog might not appear if validation fails, which is fine
            }
        }

        public void GoBackToModify()
        {
            WaitForBeingVisible(_modifyRepairsButtonBy);
            _modifyRepairsButton().Click();
        }

        public bool CheckErrorMessage(string expectedError)
        {
            WaitForBeingVisible(_errorsShownBy);
            string actualError = _driver.FindElement(_errorsShownBy).Text;
            return actualError.Contains(expectedError);
        }

        public string GetNameValue()
        {
            WaitForBeingVisible(_nameInputBy);
            return _nameInput().GetAttribute("value");
        }

        public string GetAddressValue()
        {
            WaitForBeingVisible(_addressInputBy);
            return _addressInput().GetAttribute("value");
        }
    }
}
