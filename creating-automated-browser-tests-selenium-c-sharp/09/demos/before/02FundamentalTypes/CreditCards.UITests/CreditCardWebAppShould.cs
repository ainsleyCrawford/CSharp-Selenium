using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.Windows;
using System.IO;
using ApprovalTests;
using CreditCards.UITests.PageObjectModels;

namespace CreditCards.UITests
{
    public class CreditCardWebAppShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string AboutUrl = "http://localhost:44108/Home/About";

        private const string HomeTitle = "Home Page - Credit Cards";

        [Fact]
        [Trait("Category", "Smoke")]
        public void LoadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                driver.Manage().Window.Maximize();
                DemoHelper.Pause();
                driver.Manage().Window.Minimize();
                DemoHelper.Pause();
                driver.Manage().Window.Size = new System.Drawing.Size(300, 400);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(1, 1);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(50, 50);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(100, 100);
                DemoHelper.Pause();
                driver.Manage().Window.FullScreen();

                DemoHelper.Pause(5000);

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void ReloadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                DemoHelper.Pause();

                driver.Navigate().Refresh();

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void ReloadHomePageOnBack()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                IWebElement generationTokenElement =
                    driver.FindElement(By.Id("GenerationToken"));
                string initialToken = generationTokenElement.Text;
                DemoHelper.Pause();
                driver.Navigate().GoToUrl(AboutUrl);
                DemoHelper.Pause();
                driver.Navigate().Back();
                DemoHelper.Pause();

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);

                string reloadedToken = driver.FindElement(By.Id("GenerationToken")).Text;
                Assert.NotEqual(initialToken, reloadedToken);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void ReloadHomePageOnForward()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);
                DemoHelper.Pause();

                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                driver.Navigate().Back();
                DemoHelper.Pause();

                driver.Navigate().Forward();
                DemoHelper.Pause();

                Assert.Equal(HomeTitle, driver.Title);
                Assert.Equal(HomeUrl, driver.Url);

                // TODO: assert that page was reloaded
            }
        }

        [Fact]
        public void DisplayProductsAndRates()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                var homePage = new HomePage(driver);

                DemoHelper.Pause();

                ReadOnlyCollection<IWebElement> tableCells = homePage.ProductCells;                    

                Assert.Equal("Easy Credit Card", tableCells[0].Text);
                Assert.Equal("20% APR", tableCells[1].Text);

                Assert.Equal("Silver Credit Card", tableCells[2].Text);
                Assert.Equal("18% APR", tableCells[3].Text);

                Assert.Equal("Gold Credit Card", tableCells[4].Text);
                Assert.Equal("17% APR", tableCells[5].Text);
            }
        }

        [Fact]
        public void OpenContactFooterLinkInNewTab()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                driver.FindElement(By.Id("ContactFooter")).Click();

                DemoHelper.Pause();

                ReadOnlyCollection<string> allTabs = driver.WindowHandles;
                string homePageTab = allTabs[0];
                string contactTab = allTabs[1];

                driver.SwitchTo().Window(contactTab);

                DemoHelper.Pause();

                Assert.EndsWith("/Home/Contact", driver.Url);
            }
        }

        [Fact]
        public void AlertIfLiveChatClosed()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                driver.FindElement(By.Id("LiveChat")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                Assert.Equal("Live chat is currently closed.", alert.Text);

                DemoHelper.Pause();

                alert.Accept();

                DemoHelper.Pause();
            }
        }

        [Fact]
        public void NotNavigateToAboutUsWhenCancelClicked()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Assert.Equal(HomeTitle, driver.Title);

                driver.FindElement(By.Id("LearnAboutUs")).Click();

                DemoHelper.Pause();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IAlert alertBox = wait.Until(ExpectedConditions.AlertIsPresent());

                alertBox.Dismiss();

                Assert.Equal(HomeTitle, driver.Title);
            }
        }

        [Fact]
        public void NotDisplayCookieUseMessage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                driver.Manage().Cookies.AddCookie(new Cookie("acceptedCookies", "true"));

                driver.Navigate().Refresh();

                ReadOnlyCollection<IWebElement> message = 
                    driver.FindElements(By.Id("CookiesBeingUsed"));

                Assert.Empty(message);

                Cookie cookieValue = driver.Manage().Cookies.GetCookieNamed("acceptedCookies");
                Assert.Equal("true", cookieValue.Value);

                driver.Manage().Cookies.DeleteCookieNamed("acceptedCookies");
                driver.Navigate().Refresh();
                Assert.NotNull(driver.FindElement(By.Id("CookiesBeingUsed")));
            }
        }

        [Fact]
        [UseReporter(typeof(BeyondCompare4Reporter))]
        public void RenderAboutPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);

                ITakesScreenshot screenShotDriver = (ITakesScreenshot)driver;

                Screenshot screenshot = screenShotDriver.GetScreenshot();

                screenshot.SaveAsFile("aboutpage.bmp", ScreenshotImageFormat.Bmp);

                FileInfo file = new FileInfo("aboutpage.bmp");

                Approvals.Verify(file);
            }
        }
    }
}
