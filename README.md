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
