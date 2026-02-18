using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using Reqnroll;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    [Binding]
    public class ScreenshotHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage? _page;

        public ScreenshotHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [AfterScenario(Order = 9999)]
        public async Task AfterScenario()
        {
            // Only capture screenshot if scenario failed
            if (_scenarioContext.TestError == null)
                return;

            // Try to get page from DI container (only exists for GUI tests)
            try
            {
                if (_scenarioContext.ScenarioContainer.IsRegistered<IPage>())
                {
                    _page = _scenarioContext.ScenarioContainer.Resolve<IPage>();
                    await CaptureScreenshotAsync();
                }
                else
                {
                    // This is an API test, no screenshot needed
                    Console.WriteLine("⚠️ No screenshot captured (API test)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not capture screenshot: {ex.Message}");
            }
        }

        private async Task CaptureScreenshotAsync()
        {
            if (_page == null)
                return;

            try
            {
                // Create screenshots directory
                var screenshotsDir = Path.Combine(Directory.GetCurrentDirectory(), "screenshots");
                Directory.CreateDirectory(screenshotsDir);

                // Generate filename
                var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
                var sanitizedTitle = SanitizeFileName(scenarioTitle);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var screenshotPath = Path.Combine(screenshotsDir, $"FAILED_{sanitizedTitle}_{timestamp}.png");

                // Capture full page screenshot
                await _page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = screenshotPath,
                    FullPage = true
                });

                // Attach to NUnit test context
                TestContext.AddTestAttachment(screenshotPath, $"Failed: {scenarioTitle}");

                Console.WriteLine($"📸 Screenshot captured: {screenshotPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to capture screenshot: {ex.Message}");
            }
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars));
        }
    }
}