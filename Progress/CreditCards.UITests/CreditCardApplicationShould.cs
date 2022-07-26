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
                var hP = new HomePage(driver);
                hP.NavigateTo();

                ApplicationPage aplP = hP.ClickRandomGreetingLink();

                aplP.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreeting_Using_XPATH()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var hP = new HomePage(driver);
                hP.NavigateTo();

                ApplicationPage aplP = hP.ClickRandomGreetingLink_XPath();

                aplP.EnsurePageLoaded();
            }
        }

        [Fact]
        public void BeSubmittedWhenValid()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                const string FirstName = "Ainsley", LastName = "Crawford", Number = "123456-A", Age = "28", Income = "32000", RelationshipStatus = "Single", Source = "TV";

                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();

                applicationPage.EnterFirstName(FirstName);
                applicationPage.EnterLastName(LastName);
                applicationPage.EnterFrequentFlyerNumber(Number);
                applicationPage.EnterAge(Age);
                applicationPage.EnterGrossAnnualIncome(Income);
                applicationPage.ChooseMaritalStatus(RelationshipStatus);
                applicationPage.ChooseBusinessSource(Source);
                applicationPage.AcceptTerms();
                ApplicationCompletePage applicationCompletePage = applicationPage.SubmitApplication();

                applicationCompletePage.EnsurePageLoaded();

                Assert.Equal("ReferredToHuman", applicationCompletePage.Decision);
                Assert.NotEmpty(applicationCompletePage.ReferenceNumber);
                Assert.Equal($"{FirstName} {LastName}", applicationCompletePage.FullName);
                Assert.Equal(Age, applicationCompletePage.Age);
                Assert.Equal(Income, applicationCompletePage.Income);
                Assert.Equal("Single", applicationCompletePage.RelationshipStatus);
                Assert.Equal(Source, applicationCompletePage.BusinessSource);
            }
        }

        [Fact]
        public void BeSubmittedWhenValidationErrorsCorrected()
        {
            const string FirstName = "Ainsley", LastName = "Crawford", InvalidAge = "17", ValidAge = "28", Income = "32000", RelationshipStatus = "Defacto", Source = "Email";

            using (IWebDriver driver = new ChromeDriver())
            {
                var applicationPage = new ApplicationPage(driver);
                applicationPage.NavigateTo();

                applicationPage.EnterFirstName(FirstName);
                //Don't enter surname
                applicationPage.EnterFrequentFlyerNumber("123456-A");
                applicationPage.EnterAge(InvalidAge);
                applicationPage.EnterGrossAnnualIncome(Income);
                applicationPage.ChooseMaritalStatus(RelationshipStatus);
                applicationPage.ChooseBusinessSource(Source);
                applicationPage.AcceptTerms();
                applicationPage.SubmitApplication();

                //Assert that validation failed
                Assert.Equal(2, applicationPage.ValidationErrorMessages.Count);
                Assert.Contains("Please provide a last name", applicationPage.ValidationErrorMessages);
                Assert.Contains("You must be at least 18 years old", applicationPage.ValidationErrorMessages);

                //Fix errors
                applicationPage.EnterLastName(LastName);
                applicationPage.ClearAge();
                applicationPage.EnterAge(ValidAge);

                //Resubmit form
                ApplicationCompletePage applicationCompletePage = applicationPage.SubmitApplication();

                //Check form submitted
                applicationCompletePage.EnsurePageLoaded();
                Assert.Equal("ReferredToHuman", applicationCompletePage.Decision);
                Assert.NotEmpty(applicationCompletePage.ReferenceNumber);
                Assert.Equal($"{FirstName} {LastName}", applicationCompletePage.FullName);
                Assert.Equal(ValidAge, applicationCompletePage.Age);
                Assert.Equal(Income, applicationCompletePage.Income);
                Assert.Equal("De-facto", applicationCompletePage.RelationshipStatus);
                Assert.Equal(Source, applicationCompletePage.BusinessSource);
            }
        }
    }
}
