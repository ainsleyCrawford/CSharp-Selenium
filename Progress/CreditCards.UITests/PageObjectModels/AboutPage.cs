using OpenQA.Selenium;

//So trivial that it probably shouldn't exist.

namespace CreditCards.UITests.PageObjectModels
{
    class AboutPage : Page
    {
        public AboutPage(IWebDriver driver)
        {
            Driver = driver;
        }
        protected override string PageUrl => "http://localhost:44108/Home/About";
        protected override string PageTitle => "About - Credit Cards";
        protected override string DesiredPage => "About";
    }
}
