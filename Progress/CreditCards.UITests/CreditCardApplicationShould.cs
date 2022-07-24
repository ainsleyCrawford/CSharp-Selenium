using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;
using CreditCards.UITests.PageObjectModels;

namespace CreditCards.UITests
{
    [Trait("Category", "Applications")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";

        private readonly ITestOutputHelper output;

        public CreditCardApplicationShould (ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var hP = new HomePage(driver);
                hP.NavigateTo();

                ApplicationPage aplP = hP.ClickApplyLowRateLink();

                aplP.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePageEasyApplication()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var hP = new HomePage(driver);
                hP.NavigateTo();

                driver.Manage().Window.Minimize();
                
                hP.WaitForEasyApplicationCarouselPage();

                ApplicationPage aplP = hP.ClickApplyEasyApplicationLink();

                aplP.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var hP = new HomePage(driver);

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Navigating to '{HomeUrl}'");
                hP.NavigateTo();

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Finding element using explicit wait");
                hP.WaitForEasyApplicationCustomerServicePage();
                //output.WriteLine($"{DateTime.Now.ToLongTimeString()} Element found: Displayed={applyLink.Displayed}; Enabled={applyLink.Enabled}");

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Clicking element");
                ApplicationPage aplP = hP.ClickApplyCustomerServiceLink();

                aplP.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreetingApplyLink = driver.FindElement(By.PartialLinkText("- Apply Now!"));
                randomGreetingApplyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting_Using_XPATH()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreetingApplyLink = driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));
                randomGreetingApplyLink.Click();
                
                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeSubmittedWhenValid()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys("Ainsley");
                DemoHelper.Pause();
                driver.FindElement(By.Id("LastName")).SendKeys("Crawford");
                DemoHelper.Pause();
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Age")).SendKeys("28");
                DemoHelper.Pause();
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("32000");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Single")).Click();
                DemoHelper.Pause();
                //IWebElement businessSourceSelectElement = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(driver.FindElement(By.Id("BusinessSource")));
                //Verify default option is correct
                Assert.Equal("I'd Rather Not Say", businessSource.SelectedOption.Text);
                //Get all options
                foreach(IWebElement option in businessSource.Options)
                {
                    output.WriteLine($"Value: {option.GetAttribute("value")} | Text: {option.Text}");
                }
                Assert.Equal(5, businessSource.Options.Count);
                //Select an option
                businessSource.SelectByValue("Email");
                DemoHelper.Pause();
                businessSource.SelectByText("Internet Search");
                DemoHelper.Pause();
                businessSource.SelectByIndex(4);

                driver.FindElement(By.Id("TermsAccepted")).Click();

                //driver.FindElement(By.Id("SubmitApplication")).Click();
                driver.FindElement(By.Id("Age")).Submit();

                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Ainsley Crawford", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("28", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("32000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void BeSubmittedWhenValidationErrorsCorrected()
        {
            const string firstName = "Ainsley", invalidAge = "17", validAge = "28";

            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
                //Don't enter surname
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                driver.FindElement(By.Id("Age")).SendKeys(invalidAge);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("32000");
                driver.FindElement(By.Id("Single")).Click();
                IWebElement businessSourceSelectElement = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);
                businessSource.SelectByValue("Email");
                driver.FindElement(By.Id("TermsAccepted")).Click();
                driver.FindElement(By.Id("SubmitApplication")).Click();

                //Assert that validation failed
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors > ul > li"));
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors[0].Text);
                Assert.Equal("You must be at least 18 years old", validationErrors[1].Text);

                //Fix errors
                driver.FindElement(By.Id("LastName")).SendKeys("Crawford");
                driver.FindElement(By.Id("Age")).Clear();
                driver.FindElement(By.Id("Age")).SendKeys(validAge);

                //Resubmit form
                driver.FindElement(By.Id("SubmitApplication")).Click();

                //Check form submitted
                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Ainsley Crawford", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("28", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("32000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("Email", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }
    }
}
