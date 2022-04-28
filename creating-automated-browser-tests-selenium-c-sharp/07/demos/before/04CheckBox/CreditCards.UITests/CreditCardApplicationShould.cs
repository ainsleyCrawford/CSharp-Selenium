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

                DemoHelper.Pause(5000);
            }
        }
    }
}
