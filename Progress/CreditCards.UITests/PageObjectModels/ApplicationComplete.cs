using OpenQA.Selenium;
using System;

namespace CreditCards.UITests.PageObjectModels
{
    class ApplicationCompletePage
    {
        private readonly IWebDriver Driver;
        private const string PageUrl = "http://localhost:44108/Apply";
        private const string PageTitle = "Application Complete - Credit Cards";

        public ApplicationCompletePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public string Decision => Driver.FindElement(By.Id("Decision")).Text;
        public string ReferenceNumber => Driver.FindElement(By.Id("ReferenceNumber")).Text;
        public string FullName => Driver.FindElement(By.Id("FullName")).Text;
        public string Age => Driver.FindElement(By.Id("Age")).Text;
        public string Income => Driver.FindElement(By.Id("Income")).Text;
        public string RelationshipStatus => Driver.FindElement(By.Id("RelationshipStatus")).Text;
        public string BusinessSource => Driver.FindElement(By.Id("BusinessSource")).Text;

        public void EnsurePageLoaded(bool onlyCheckUrlStartsWithExpectedText = true)
        {
            bool urlIsCorrect;
            if (onlyCheckUrlStartsWithExpectedText)
            {
                urlIsCorrect = Driver.Url.StartsWith(PageUrl);
            }
            else
            {
                urlIsCorrect = Driver.Url == PageUrl;
            }

            bool pageHasLoaded = urlIsCorrect && (Driver.Title == PageTitle);

            if (!pageHasLoaded)
            {
                throw new Exception($"Failed to load Application Complete Page.\r\nPage URL = '{Driver.Url}'.\r\nPage Source: \r\n {Driver.PageSource}");
            }
        }
    }
}
