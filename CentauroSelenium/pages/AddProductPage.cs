using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;


namespace CentauroSelenium.Pages
{
    public class AddProductPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _nombreProductoXpath;
        private readonly string _buscarBtnXpath;
        private readonly string _primerCheckboxXpath;
        private readonly string _siguienteBtnXpath;
        private readonly string _medidaComercialXpath;
        private readonly string _precioUnitarioXpath;
        private readonly string _dropdownMedidaXpath;
        private readonly string _detalleXpath;
        private readonly string _guardarBtnXpath;

        public string DetalleIngresado { get; private set; } = "";

        public AddProductPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            dynamic locators = JsonReader.LoadJson("locators/dashboard_locators.json");
            var add = locators.add_product_page;
            _nombreProductoXpath = add.nombre_producto;
            _buscarBtnXpath = add.buscar_btn;
            _primerCheckboxXpath = add.primer_checkbox;
            _siguienteBtnXpath = add.siguiente_btn;
            _medidaComercialXpath = add.medida_comercial;
            _precioUnitarioXpath = add.precio_unitario;
            _dropdownMedidaXpath = add.dropdown_medida;
            _detalleXpath = add.detalle;
            _guardarBtnXpath = add.guardar_btn;
        }

        private IWebElement WaitForElement(string xpath, int timeout = 10)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout))
                .Until(drv => drv.FindElement(By.XPath(xpath)));
        }

        public void IngresarNombreProducto(string nombre)
        {
            var input = WaitForElement(_nombreProductoXpath);
            input.Clear();
            input.SendKeys(nombre);
        }

        public void ClickBuscar()
        {
            WaitForElement(_buscarBtnXpath).Click();
        }

        public void SeleccionarPrimerCheckbox()
        {
            var checkbox = WaitForElement(_primerCheckboxXpath);
            if (!checkbox.Selected)
                checkbox.Click();
        }

        public void ClickSiguiente()
        {
            WaitForElement(_siguienteBtnXpath).Click();
        }

        public void IngresarMedidaComercial(string medida)
        {
            var input = WaitForElement(_medidaComercialXpath);
            input.Clear();
            input.SendKeys(medida);
        }

        public void IngresarPrecioUnitario(decimal precio)
        {
            var input = WaitForElement(_precioUnitarioXpath);
            input.Clear();
            input.SendKeys(precio.ToString());
        }

        public void SeleccionarMedidaDropdown(int index = 1)
        {
            var select = new SelectElement(WaitForElement(_dropdownMedidaXpath));
            if (select.Options.Count > 1)
                select.SelectByIndex(index);
        }

        public void IngresarDetalle(string detalle)
        {
            var input = WaitForElement(_detalleXpath);
            input.Clear();
            input.SendKeys(detalle);
            DetalleIngresado = detalle;
        }

        public void ClickGuardar()
        {
            WaitForElement(_guardarBtnXpath).Click();
        }
    }
}