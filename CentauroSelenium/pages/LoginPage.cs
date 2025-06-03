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

        public void EnterUsername(string username)
        {
            var usernameField = _wait.Until(drv => drv.FindElement(By.XPath(_usernameXpath)));
            usernameField.Clear();
            usernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var passwordField = _wait.Until(drv => drv.FindElement(By.XPath(_passwordXpath)));
            passwordField.Clear();
            passwordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            var loginButton = _wait.Until(drv => drv.FindElement(By.XPath(_loginButtonXpath)));
            loginButton.Click();
        }
    }
}