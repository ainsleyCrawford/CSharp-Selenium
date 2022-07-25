using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ReadOnlyCollection<string> ValidationErrorMessages
        {
            get
            {
                return Driver.FindElements(
                    By.CssSelector(".validation-summary-errors > ul > li"))
                    .Select(x => x.Text)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public void ClearAge() => Driver.FindElement(By.Id("Age")).Clear();

        public void EnterFirstName(string firstName) => Driver.FindElement(By.Id("FirstName")).SendKeys(firstName);

        public void EnterLastName(string lastName) => Driver.FindElement(By.Id("LastName")).SendKeys(lastName);

        public void EnterFrequentFlyerNumber(string number) => Driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(number);

        public void EnterAge(string age) => Driver.FindElement(By.Id("Age")).SendKeys(age);

        public void EnterGrossAnnualIncome(string income) => Driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(income);

        public void ChooseMaritalStatusSingle() => Driver.FindElement(By.Id("Single")).Click();

        public void ChooseBusinessSourceTV()
        {
            SelectElement businessSource = new SelectElement(Driver.FindElement(By.Id("BusinessSource")));
            businessSource.SelectByValue("TV");
        }
        
        public void ChooseBusinessSourceEmail()
        {
            SelectElement businessSource = new SelectElement(Driver.FindElement(By.Id("BusinessSource")));
            businessSource.SelectByValue("Email");
        }

        public void AcceptTerms() => Driver.FindElement(By.Id("TermsAccepted")).Click();

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
                throw new Exception($"Failed to load Application Page.\r\nPage URL = '{Driver.Url}'.\r\nPage Source: \r\n {Driver.PageSource}");
            }
        }
    }
}
