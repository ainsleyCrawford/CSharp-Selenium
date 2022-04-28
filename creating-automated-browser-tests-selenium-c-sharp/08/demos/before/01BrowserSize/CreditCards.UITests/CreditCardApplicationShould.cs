using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;

namespace CreditCards.UITests
{
    [Trait("Category", "Applications")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";

        private readonly ITestOutputHelper output;

        public CreditCardApplicationShould(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BeInitiatedFromHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement applyLink = driver.FindElement(By.Name("ApplyLowRate"));
                applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement carouselNext =
                    driver.FindElement(By.CssSelector("[data-slide='next']"));
                carouselNext.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                IWebElement applyLink =
                    wait.Until((d) => d.FindElement(By.LinkText("Easy: Apply Now!")));
                applyLink.Click();

                //IWebElement applyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                //applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication_Prebuilt_Conditions()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                WebDriverWait wait =
                    new WebDriverWait(driver, TimeSpan.FromSeconds(11));
                IWebElement applyLink =
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Easy: Apply Now!")));
                applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Navigating to '{HomeUrl}'");
                driver.Navigate().GoToUrl(HomeUrl);

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Finding element using explicit wait");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(35));

                IWebElement applyLink =
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("customer-service-apply-now")));

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Found element Displayed={applyLink.Displayed} Enabled={applyLink.Enabled}");
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Clicking element");
                applyLink.Click();

                DemoHelper.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreetingApplyLink =
                    driver.FindElement(By.PartialLinkText("- Apply Now!"));
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

                IWebElement randomGreetingApplyLink =
                    driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));
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

                driver.FindElement(By.Id("FirstName")).SendKeys("Sarah");
                DemoHelper.Pause();
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                DemoHelper.Pause();
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Age")).SendKeys("18");
                DemoHelper.Pause();
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                DemoHelper.Pause();
                driver.FindElement(By.Id("Single")).Click();
                DemoHelper.Pause();
                IWebElement businessSourceSelectElement = 
                    driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);
                // Check default selected option is correct
                Assert.Equal("I'd Rather Not Say", businessSource.SelectedOption.Text);
                // Get all the available options
                foreach(IWebElement option in businessSource.Options)
                {
                    output.WriteLine($"Value: {option.GetAttribute("value")} Text: {option.Text}");
                }
                Assert.Equal(5, businessSource.Options.Count);
                // Select an option
                businessSource.SelectByValue("Email");
                DemoHelper.Pause();
                businessSource.SelectByText("Internet Search");
                DemoHelper.Pause();
                businessSource.SelectByIndex(4); // Zero-based   

                driver.FindElement(By.Id("TermsAccepted")).Click();

                //driver.FindElement(By.Id("SubmitApplication")).Click();
                driver.FindElement(By.Id("Single")).Submit();

                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void BeSubmittedWhenValidationErrorsCorrected()
        {
            const string firstName = "Sarah";
            const string invalidAge = "17";
            const string validAge = "18";

            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
                // Don't enter lastname
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("123456-A");
                driver.FindElement(By.Id("Age")).SendKeys(invalidAge);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("50000");
                driver.FindElement(By.Id("Single")).Click();
                IWebElement businessSourceSelectElement =
                        driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);
                businessSource.SelectByValue("Email");
                driver.FindElement(By.Id("TermsAccepted")).Click();
                driver.FindElement(By.Id("SubmitApplication")).Click();

                // Assert that validation failed                
                var validationErrors =
                    driver.FindElements(By.CssSelector(".validation-summary-errors > ul > li"));
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors[0].Text);
                Assert.Equal("You must be at least 18 years old", validationErrors[1].Text);

                // Fix errors
                driver.FindElement(By.Id("LastName")).SendKeys("Smith");
                driver.FindElement(By.Id("Age")).Clear();
                driver.FindElement(By.Id("Age")).SendKeys(validAge);

                // Resubmit form
                driver.FindElement(By.Id("SubmitApplication")).Click();

                // Check form submitted
                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal("Sarah Smith", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal("18", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("50000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("Email", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }
    }
}
