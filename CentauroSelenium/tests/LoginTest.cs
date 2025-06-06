using NUnit.Framework;
using OpenQA.Selenium;
using CentauroSelenium.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace CentauroSelenium.Tests
{
    public class LoginTest
    {
        private IWebDriver _driver;
        private string _url;
        private string _username;
        private string _password;

        [SetUp]
        public void SetUp()
        {
            dynamic creds = JsonReader.LoadJson("configs/credentials.json");
            _url = creds.brand_url;
            _username = creds.credentials.username;
            _password = creds.credentials.password;

            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void Login_With_Valid_Credentials_Should_Succeed()
        {
            _driver.Navigate().GoToUrl(_url);

            var loginPage = new LoginPage(_driver);

            Assert.That(loginPage.EnterUsername(_username), Is.True, "No se pudo ingresar el usuario.");
            Assert.That(loginPage.EnterPassword(_password), Is.True, "No se pudo ingresar la contraseña.");
            Assert.That(loginPage.ClickLogin(), Is.True, "No se pudo hacer click en el botón de login.");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            bool loggedIn = wait.Until(drv => !drv.Url.Contains("Login"));

            Assert.That(loggedIn, Is.True, "No se realizó el login correctamente.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}