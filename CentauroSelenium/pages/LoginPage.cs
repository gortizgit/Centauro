using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CentauroSelenium.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _usernameXpath;
        private readonly string _passwordXpath;
        private readonly string _loginButtonXpath;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            dynamic locators = JsonReader.LoadJson("locators/login_locators.json");
            _usernameXpath = locators.login_page.username;
            _passwordXpath = locators.login_page.password;
            _loginButtonXpath = locators.login_page.login_button;
        }

        private bool WaitForElementVisible(string xpath)
        {
            try
            {
                return _wait.Until(driver =>
                {
                    var element = driver.FindElement(By.XPath(xpath));
                    bool isDisplayed = (bool)((IJavaScriptExecutor)driver)
                        .ExecuteScript("return arguments[0].offsetParent !== null && arguments[0].offsetWidth > 0 && arguments[0].offsetHeight > 0;", element);
                    return isDisplayed;
                });
            }
            catch
            {
                return false;
            }
        }

        public bool EnterUsername(string username)
        {
            if (WaitForElementVisible(_usernameXpath))
            {
                var usernameField = _driver.FindElement(By.XPath(_usernameXpath));
                usernameField.Clear();
                usernameField.SendKeys(username);
                return true;
            }
            return false;
        }

        public bool EnterPassword(string password)
        {
            if (WaitForElementVisible(_passwordXpath))
            {
                var passwordField = _driver.FindElement(By.XPath(_passwordXpath));
                passwordField.Clear();
                passwordField.SendKeys(password);
                return true;
            }
            return false;
        }

        public bool ClickLogin()
        {
            if (WaitForElementVisible(_loginButtonXpath))
            {
                var loginButton = _driver.FindElement(By.XPath(_loginButtonXpath));
                loginButton.Click();
                return true;
            }
            return false;
        }
    }
}