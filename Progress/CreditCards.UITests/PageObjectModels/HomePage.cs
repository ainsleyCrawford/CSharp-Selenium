using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CreditCards.UITests.PageObjectModels
{
    class HomePage
    {
        private readonly IWebDriver Driver;
        private const string PageUrl = "http://localhost:44108/";
        private const string PageTitle = "Home Page - Credit Cards";

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public ReadOnlyCollection<(string name, string interestRate)> Products
        {
            get
            {
                var products = new List<(string name, string interestRate)>();

                var productCells = Driver.FindElements(By.TagName("td"));

                for (int i = 0; i < productCells.Count; i +=2)
                {
                    string name = productCells[i].Text;
                    string interestRate = productCells[i + 1].Text;
                    products.Add((name, interestRate));
                }

                return products.AsReadOnly();
            }
        }

        public string GenerationToken => Driver.FindElement(By.Id("GenerationToken")).Text;

        public bool IsCookieMessagePresent => Driver.FindElements(By.Id("CookiesBeingUsed")).Any();

        public void ClickContactFooterLink() => Driver.FindElement(By.Id("ContactFooter")).Click();

        public void ClickLiveChatFooterLink() => Driver.FindElement(By.Id("LiveChat")).Click();

        public void ClickLearnAboutUsLink() => Driver.FindElement(By.Id("LearnAboutUs")).Click();

        public ApplicationPage ClickApplyLowRateLink()
        {
            Driver.FindElement(By.Name("ApplyLowRate")).Click();
            return new ApplicationPage(Driver);
        }

        public void WaitForEasyApplicationCarouselPage()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(11));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("Easy: Apply Now!")));
        }

        public ApplicationPage ClickApplyEasyApplicationLink()
        {
            Driver.FindElement(By.LinkText("Easy: Apply Now!")).Click();
            return new ApplicationPage(Driver);
        }

        public void WaitForEasyApplicationCustomerServicePage()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(35));
            IWebElement applyLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("customer-service-apply-now")));

        }

        public ApplicationPage ClickApplyCustomerServiceLink()
        {
            Driver.FindElement(By.ClassName("customer-service-apply-now")).Click();
            return new ApplicationPage(Driver);
        }
        
        public ApplicationPage ClickRandomGreetingLink()
        {
            Driver.FindElement(By.PartialLinkText("- Apply Now!")).Click();
            return new ApplicationPage(Driver);
        }
        
        public ApplicationPage ClickRandomGreetingLink_XPath()
        {
            Driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]")).Click();
            return new ApplicationPage(Driver);
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
                throw new Exception($"Failed to load Home Page.\r\nPage URL = '{Driver.Url}'.\r\nPage Source: \r\n {Driver.PageSource}");
            }
        }
    }
}
