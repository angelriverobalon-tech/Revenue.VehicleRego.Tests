using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    [Binding]
    public class ReqnrollHooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IPage? _page;

        public ReqnrollHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public async Task BeforeScenario()
        {
            // Check if this is a GUI test
            var isGuiTest = _scenarioContext.ScenarioInfo.Tags.Contains("GUI") ||
                           _scenarioContext.ScenarioInfo.Tags.Contains("UI") ||
                           _scenarioContext.ScenarioInfo.Tags.Contains("Playwright");

            // Only initialize Playwright for GUI tests
            if (isGuiTest)
            {
                var headlessStr = ConfigManager.GetConfigValue("PLAYWRIGHT_HEADLESS", "true");
                var headless = !string.Equals(headlessStr, "false", StringComparison.OrdinalIgnoreCase);

                await PlaywrightDriver.InitAsync(headless);

                // Create a new page for this scenario
                _page = await PlaywrightDriver.NewPageAsync();

                // Register the page instance in the DI container
                _objectContainer.RegisterInstanceAs<IPage>(_page);

                Console.WriteLine("🌐 Playwright page initialized for GUI test");
            }
            else
            {
                Console.WriteLine("📡 API test - Playwright not initialized");
            }
        }

        [AfterScenario(Order = 10000)]
        public async Task AfterScenario()
        {
            // Close the page after GUI scenario
            if (_page != null)
            {
                try
                {
                    await _page.CloseAsync();
                    Console.WriteLine("🌐 Playwright page closed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error closing page: {ex.Message}");
                }
            }
        }
    }
}
