using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using System.IO;

namespace CentauroSelenium.Drivers
{
    public class WebDriverSetup
    {
        public IWebDriver InitializeWebDriver()
        {
            // Cargar configuración desde JSON
            var config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("configs/test_config.json"));

            var options = new ChromeOptions();
            if ((bool)config["test_settings"]["headless_mode"])
            {
                options.AddArgument("--headless");
            }

            options.AddArgument("--start-maximized"); // Maximizar navegador

            // Usa el ChromeDriver del paquete NuGet, sin ruta
            return new ChromeDriver(options);
        }
    }
}