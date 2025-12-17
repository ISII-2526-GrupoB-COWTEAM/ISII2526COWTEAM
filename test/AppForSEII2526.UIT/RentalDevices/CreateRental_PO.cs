using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.RentalDevices
{
    internal class CreateRental_PO : PageObject
    {
        private By _nameInputBy = By.Id("Name");
        private By _surnameInputBy = By.Id("Surname");
        private By _addressInputBy = By.Id("DeliveryAddress");
        private By _paymentSelectBy = By.Id("PaymentMethod");
        private By _dateFromInputBy = By.Id("DateFrom");
        private By _dateToInputBy = By.Id("DateTo");
        private By _submitButtonBy = By.Id("Submit");
        private By _modifyDevicesButtonBy = By.Id("ModifyDevices");
        private By _errorsShownBy = By.Id("ErrorsShown");
        private By _dialogOkButtonBy = By.Id("Button_DialogOK");

        private IWebElement _nameInput() => _driver.FindElement(_nameInputBy);
        private IWebElement _surnameInput() => _driver.FindElement(_surnameInputBy);
        private IWebElement _addressInput() => _driver.FindElement(_addressInputBy);
        private IWebElement _paymentSelect() => _driver.FindElement(_paymentSelectBy);
        private IWebElement _dateFromInput() => _driver.FindElement(_dateFromInputBy);
        private IWebElement _dateToInput() => _driver.FindElement(_dateToInputBy);
        private IWebElement _submitButton() => _driver.FindElement(_submitButtonBy);
        private IWebElement _modifyDevicesButton() => _driver.FindElement(_modifyDevicesButtonBy);

        public CreateRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
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

        // Helper to set dates relative to Today
        public void SetDates(int daysFromStart, int daysFromEnd)
        {
             WaitForBeingVisible(_dateFromInputBy);
             var start = DateTime.Today.AddDays(daysFromStart);
             var end = DateTime.Today.AddDays(daysFromEnd);
             
             // Check format required by InputDate. Usually local date string or yyyy-MM-dd
             // Selenium SendKeys with date input often accepts yyyy-mm-dd or local format like dd-mm-yyyy
             // Best bet is dd/MM/yyyy assuming Spanish locale or try yyyy-MM-dd
             string format = "dd/MM/yyyy";
             
             _dateFromInput().SendKeys(start.ToString(format));
             _dateToInput().SendKeys(end.ToString(format));
        }
        
        // Direct string setter for specific error testing
        public void SetDateFrom(string date)
        {
            WaitForBeingVisible(_dateFromInputBy);
            _dateFromInput().SendKeys(date);
        }
         public void SetDateTo(string date)
        {
            WaitForBeingVisible(_dateToInputBy);
            _dateToInput().SendKeys(date);
        }


        public void SubmitRental()
        {
            WaitForBeingVisible(_submitButtonBy);
            _submitButton().Click();

            try
            {
                WaitForBeingVisible(_dialogOkButtonBy);
                _driver.FindElement(_dialogOkButtonBy).Click();
            }
            catch (WebDriverTimeoutException)
            {
                // Dialog did not appear
            }
        }

        public void GoBackToModify()
        {
            WaitForBeingVisible(_modifyDevicesButtonBy);
            _modifyDevicesButton().Click();
        }

        public bool CheckErrorMessage(string fragment)
        {
            try {
                WaitForBeingVisible(_errorsShownBy);
                string err = _driver.FindElement(_errorsShownBy).Text;
                return err.Contains(fragment);
            } catch {
                // Check ValidationSummary too if ErrorsShown is empty?
                // The ValidationSummary has class "validation-summary-errors" usually
                return _driver.PageSource.Contains(fragment);
            }
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
