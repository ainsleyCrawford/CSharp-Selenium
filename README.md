# Tutorial-Creating-Automated-Browser-Tests-wth-Selenium-in-C#
Pluralsight course by Jason Roberts

## Page Object Model
Provides test code with a logical view of the user interface while abstracting away the low-level details of the user interface imlementation.

### POMs sit between the test code and the the WebDriver API:
Test framework (NUnit) --> Test code (C#) <--> **POMs** <--> IWebDriver API (NuGet) <--> Browser driver (ChromeDriver) <--> Browser (Chrome) <--> Web server hosting the app (IIS)

### POM Benefits
- Cost
  - Avoid duplication
  - Robust to UI changes
- Readability
  - Highlight test logic (not low-level details)
  - Reduced HTML implementation clutter
- Use of Fundamental .Net types
  - e.g., strings over IWebElements
- Abstract away IWebDriver interface
  - i.e., replace `driver.FindElement()`
- Navigation between POMs
  - Each page has a POM

### POM Considerations
- Size
  - e.g. a dashboard page could be very large
  - Separate into Page Component Object Models
- No Asserts or Exceptions
  - Excepting page load verification
  - `public void EnsurePageLoaded()`
- Create reusable "user behaviour" methods
  - `private void EnterApplicationDetails(default params)`

## Good Practices
- Use POMs for all non-trivial tests
- Check page is loaded before interacting
- Element locators
  - Most reliable
  - Most simple (e.g. Id)
- Use configuration files (e.g. servers and ports)
- Explicit waits > Thread.Sleep
- Independent test ordering (i.e., test isolation)
- Execute as part of CI build
- Usually test features, not visuals
- Consider browser update management on test machines
- Test case/data preparation
