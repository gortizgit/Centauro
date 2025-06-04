using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace CentauroSelenium.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _dashboardXpath;
        private readonly string _productosXpath;
        private readonly string _nuevoProductoXpath;
        private readonly string _informacionXpath;
        private readonly string _buscarDetalleXpath;
        private readonly string _resultadoDetalleXpath;

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            dynamic locators = JsonReader.LoadJson("locators/dashboard_locators.json");
            _dashboardXpath = locators.dashboard_page.dashboard;
            _productosXpath = locators.dashboard_page.productos_servicios;
            _nuevoProductoXpath = locators.dashboard_page.nuevo_producto;
            _informacionXpath = locators.dashboard_page.informacion;
            _buscarDetalleXpath = locators.dashboard_page.buscar_detalle;
            _resultadoDetalleXpath = locators.dashboard_page.resultado_detalle;
        }

        private IWebElement? WaitForElementVisible(string xpath, int timeoutSeconds = 15)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(driver =>
                {
                    try
                    {
                        var elements = driver.FindElements(By.XPath(xpath));
                        foreach (var element in elements)
                        {
                            bool isDisplayed = (bool)((IJavaScriptExecutor)driver)
                                .ExecuteScript(
                                    "return arguments[0].offsetParent !== null && arguments[0].offsetWidth > 0 && arguments[0].offsetHeight > 0;",
                                    element
                                );
                            if (isDisplayed)
                                return element;
                        }
                        return null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public bool ClickDashboard()
        {
            var element = WaitForElementVisible(_dashboardXpath);
            if (element == null)
                return false;
            element.Click();
            return WaitForElementVisible(_productosXpath, 10) != null;
        }

        public bool ClickProductosYServicios()
        {
            var element = WaitForElementVisible(_productosXpath);
            if (element == null)
                return false;
            element.Click();

            var nuevoProducto = WaitForElementVisible(_nuevoProductoXpath, 10);
            var aviso = WaitForElementVisible("//span[contains(@class,'title-warning-error-plan')]", 2);

            if (aviso != null)
            {
                var infoBtn = WaitForElementVisible(_informacionXpath, 5);
                if (infoBtn != null)
                    infoBtn.Click();
                element = WaitForElementVisible(_productosXpath, 10);
                if (element == null)
                    return false;
                element.Click();
                nuevoProducto = WaitForElementVisible(_nuevoProductoXpath, 10);
                aviso = WaitForElementVisible("//span[contains(@class,'title-warning-error-plan')]", 2);
                if (aviso != null)
                    return false;
            }

            return nuevoProducto != null;
        }

        public bool ClickNuevoProducto()
        {
            var element = WaitForElementVisible(_nuevoProductoXpath);
            if (element == null)
                return false;
            element.Click();
            return true;
        }
        public void BuscarDetalleProducto(string detalle)
        {
            var buscarInput = WaitForElementVisible(_buscarDetalleXpath, 10);
            if (buscarInput == null)
                throw new Exception("No se encontró el campo de búsqueda de detalle.");
            buscarInput.Clear();
            buscarInput.SendKeys(detalle);
        }

        public bool ExisteDetalleEnResultados(string detalle)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            return wait.Until(driver =>
            {
                var spans = driver.FindElements(By.XPath(_resultadoDetalleXpath));
                return spans.Any(span => span.Text.Trim().Equals(detalle, StringComparison.OrdinalIgnoreCase));
            });
        }
    }
}