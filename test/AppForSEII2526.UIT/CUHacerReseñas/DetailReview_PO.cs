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
    public class DetailReview_PO : PageObject
    {
        private By _reviewDetailsBy = By.Id("ReviewItems");

        public DetailReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReviewDetails(List<string[]> expectedData)
        {
             // Expected Data: [DeviceName, Rating, Comment] or similar structure
            return CheckBodyTable(expectedData, _reviewDetailsBy);
        }

         public bool CheckClientInfo(string name, string country)
        {
            WaitForBeingVisible(_reviewDetailsBy);
            return _driver.PageSource.Contains(name) && _driver.PageSource.Contains(country);
        }
    }
}
