using NUnit.Framework;
using OpenQA.Selenium;
using CentauroSelenium.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace CentauroSelenium.Tests
{
    public class AddNewProductTest
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
        public void Add_New_Product_Flow()
        {
            _driver.Navigate().GoToUrl(_url);
            var loginPage = new LoginPage(_driver);
            Assert.DoesNotThrow(() => loginPage.EnterUsername(_username), "No se pudo ingresar el usuario.");
            Assert.DoesNotThrow(() => loginPage.EnterPassword(_password), "No se pudo ingresar la contraseña.");
            Assert.DoesNotThrow(() => loginPage.ClickLogin(), "No se pudo hacer click en Login.");

            var dashboardPage = new DashboardPage(_driver);

            Assert.That(dashboardPage.ClickDashboard(), Is.True, "No se pudo hacer click en Dashboard correctamente.");
            Assert.That(dashboardPage.ClickProductosYServicios(), Is.True, "No se pudo hacer click en Productos y Servicios correctamente.");
            Assert.That(dashboardPage.ClickNuevoProducto(), Is.True, "No se pudo hacer click en Nuevo Producto correctamente.");

            var addProductPage = new AddProductPage(_driver);

            Assert.DoesNotThrow(() => addProductPage.IngresarNombreProducto("t"), "No se pudo ingresar el nombre del producto.");
            Assert.DoesNotThrow(() => addProductPage.ClickBuscar(), "No se pudo hacer click en Buscar.");
            Assert.DoesNotThrow(() => addProductPage.SeleccionarPrimerCheckbox(), "No se pudo seleccionar el primer checkbox.");
            Assert.DoesNotThrow(() => addProductPage.ClickSiguiente(), "No se pudo hacer click en Siguiente.");
            Assert.DoesNotThrow(() => addProductPage.IngresarMedidaComercial("Medida" + Guid.NewGuid().ToString("N").Substring(0, 5)), "No se pudo ingresar la medida comercial.");
            Assert.DoesNotThrow(() => addProductPage.IngresarPrecioUnitario(100), "No se pudo ingresar el precio unitario.");
            Assert.DoesNotThrow(() => addProductPage.SeleccionarMedidaDropdown(), "No se pudo seleccionar una medida en el dropdown.");
            Assert.DoesNotThrow(() => addProductPage.IngresarDetalle("Detalle " + Guid.NewGuid().ToString("N").Substring(0, 8)), "No se pudo ingresar el detalle.");
            Assert.DoesNotThrow(() => addProductPage.ClickGuardar(), "No se pudo hacer click en Guardar.");

            string detalle = addProductPage.DetalleIngresado;
            Assert.DoesNotThrow(() => dashboardPage.BuscarDetalleProducto(detalle), "No se pudo buscar el detalle del producto.");
            Assert.That(dashboardPage.ExisteDetalleEnResultados(detalle), Is.True, "El producto no se creó correctamente.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}