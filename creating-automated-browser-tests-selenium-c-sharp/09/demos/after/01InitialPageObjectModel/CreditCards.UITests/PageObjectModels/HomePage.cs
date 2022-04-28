using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace CreditCards.UITests.PageObjectModels
{
    class HomePage
    {
        private readonly IWebDriver Driver;

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public ReadOnlyCollection<IWebElement> ProductCells
        {
            get
            {
                return Driver.FindElements(By.TagName("td"));
            }
        }
    }
}
