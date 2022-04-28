using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CreditCards.UITests.PageObjectModels
{
    class ApplicationPage
    {
        private readonly IWebDriver Driver;
        private const string PageUrl = "http://localhost:44108/Apply";
        private const string PageTitle = "Credit Card Application - Credit Cards";

        public ApplicationPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void AcceptTerms() => Driver.FindElement(By.Id("TermsAccepted")).Click();

        public void EnterFirstName(string firstName) => Driver.FindElement(By.Id("FirstName")).SendKeys(firstName);

        public void EnterLastName(string lastName) => Driver.FindElement(By.Id("LastName")).SendKeys(lastName);

        public void EnterFrequentFlyerNumber(string number) => Driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(number);

        public void EnterAge(string age) => Driver.FindElement(By.Id("Age")).SendKeys(age);

        public void EnterGrossAnnualIncome(string income) => Driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(income);

        public void ChooseMaritalStatusSingle() => Driver.FindElement(By.Id("Single")).Click();

        public void ChooseBusinessSourceTV()
        {
            IWebElement businessSourceSelectElement = Driver.FindElement(By.Id("BusinessSource"));
            SelectElement businessSource = new SelectElement(businessSourceSelectElement);
            businessSource.SelectByValue("TV");
        }

        public ApplicationCompletePage SubmitApplication()
        {
            Driver.FindElement(By.Id("SubmitApplication")).Click();
            return new ApplicationCompletePage(Driver);
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(PageUrl);
            EnsurePageLoaded();
        }

        /// <summary>
        /// Checks that the URL and page title are as expected
        /// </summary>
        /// <param name="onlyCheckUrlStartsWithExpectedText">Set to false to do an exact match of URL. Set to true to ignore fragments, query string, etc at end of browser URL</param>
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
                throw new Exception($"Failed to load page. Page URL = '{Driver.Url}' Page Source: \r\n {Driver.PageSource}");
            }
        }
    }
}