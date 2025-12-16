using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CUHacerReseñas
{
    public class CreateReview_PO : PageObject
    {
        private By _titleInputBy = By.Id("Title");
        private By _nameInputBy = By.Id("UserName");
        private By _countryInputBy = By.Id("Country");
        private By _submitButtonBy = By.Id("Submit");
        private By _modifySelectionButtonBy = By.Id("ModifyDevices");
        private By _errorsShownBy = By.Id("ErrorsShown");

        private IWebElement _titleInput() => _driver.FindElement(_titleInputBy);
        private IWebElement _nameInput() => _driver.FindElement(_nameInputBy);
        private IWebElement _countryInput() => _driver.FindElement(_countryInputBy);
        private IWebElement _submitButton() => _driver.FindElement(_submitButtonBy);
        private IWebElement _modifySelectionButton() => _driver.FindElement(_modifySelectionButtonBy);

        public CreateReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SetTitle(string title)
        {
            WaitForBeingVisible(_titleInputBy);
            _titleInput().Clear();
            _titleInput().SendKeys(title);
        }

        public void SetName(string name)
        {
            WaitForBeingVisible(_nameInputBy);
            _nameInput().Clear();
            _nameInput().SendKeys(name);
        }

        public void SetCountry(string country)
        {
            WaitForBeingVisible(_countryInputBy);
            _countryInput().Clear();
            _countryInput().SendKeys(country);
        }

        public void SetRatingByIndex(int index, string rating)
        {
            // Assuming inputs for rating have IDs or names that can be found in order
            var inputs = _driver.FindElements(By.XPath("//input[contains(@id, 'rating_')]"));
            if (index < inputs.Count)
            {
                inputs[index].Clear();
                inputs[index].SendKeys(rating);
            }
            else
            {
                throw new NoSuchElementException($"Rating input at index {index} not found. Found {inputs.Count} inputs.");
            }
        }

        public void SetCommentByIndex(int index, string comment)
        {
            // Found <InputText ... id="comments_..." /> which renders as input
            var inputs = _driver.FindElements(By.XPath("//input[contains(@id, 'comments_')]"));
            if (index < inputs.Count)
            {
                inputs[index].Clear();
                inputs[index].SendKeys(comment);
            }
            else
            {
                 // Try textarea just in case, but Razor InputText usually renders input type="text"
                var textareas = _driver.FindElements(By.XPath("//textarea[contains(@id, 'comments_')]"));
                 if (index < textareas.Count)
                 {
                    textareas[index].Clear();
                    textareas[index].SendKeys(comment);
                    return;
                 }
                throw new NoSuchElementException($"Comment input at index {index} not found. Found {inputs.Count} inputs.");
            }
        }

        private By _dialogOkButtonBy = By.Id("Button_DialogOK");
        private By _validationSummaryBy = By.CssSelector("div.validation-errors ul, ul.validation-errors, .validation-message"); 
        // ValidationSummary often renders as a UL inside a div, or we can look for the alert class.
        // In the razor: <ValidationSummary class="row alert alert-danger" /> 
        // This usually renders as <div class="row alert alert-danger"><ul><li>Error</li></ul></div>
        private By _alertDangerBy = By.CssSelector(".alert.alert-danger");

        public void SubmitReview()
        {
            WaitForBeingVisible(_submitButtonBy);
            _submitButton().Click();

             try
            {
                // Wait briefly for dialog
                WaitForBeingVisible(_dialogOkButtonBy);
                _driver.FindElement(_dialogOkButtonBy).Click();
            }
            catch (WebDriverTimeoutException)
            {
                // Dialog might not appear if validation fails
            }
        }

        public void GoBackToModify()
        {
            WaitForBeingVisible(_modifySelectionButtonBy);
            _modifySelectionButton().Click();
        }

        public bool CheckErrorMessage(string expectedError)
        {
            // 1. Check ErrorsShown (often server-side general errors)
            try {
                WaitForBeingVisible(_errorsShownBy);
                 if (_driver.FindElement(_errorsShownBy).Text.Contains(expectedError)) return true;
            } catch {}

            // 2. Check ValidationSummary or alert-danger (standard blazor validation)
            try {
                var alerts = _driver.FindElements(_alertDangerBy);
                foreach (var alert in alerts)
                {
                    if (alert.Displayed && alert.Text.Contains(expectedError)) return true;
                }
            } catch {}

            // 3. Last resort: check if text exists in standard validation message elements
             // <div class="validation-message">...</div>
            var validationMessages = _driver.FindElements(By.CssSelector(".validation-message"));
            foreach(var msg in validationMessages) {
                 if (msg.Displayed && msg.Text.Contains(expectedError)) return true;
            }

            // 4. Fallback: PageSource check (least robust but catches uncategorized text)
             // Sometimes validation errors are just plain text or spans
             // Be careful with false positives (e.g. if expectedError is also the input value?)
             // Assuming validation message is distinct.
            return _driver.PageSource.Contains(expectedError);
        }

        public string GetNameValue()
        {
            WaitForBeingVisible(_nameInputBy);
            return _nameInput().GetAttribute("value");
        }

        public string GetCountryValue()
        {
            WaitForBeingVisible(_countryInputBy);
            return _countryInput().GetAttribute("value");
        }
        
         public string GetTitleValue()
        {
             WaitForBeingVisible(_titleInputBy);
            return _titleInput().GetAttribute("value");
        }
    }
}
