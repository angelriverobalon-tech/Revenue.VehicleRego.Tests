using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Revenue.Tests.VehicleRego.BDD.Support
{
    public static class PlaywrightDriver
    {
        private static IPlaywright? _playwright;
        private static IBrowser? _browser;
        private static readonly object _lock = new object();

        public static async Task InitAsync(bool headless = true)
        {
            lock (_lock)
            {
                if (_playwright != null)
                    return;
            }

            _playwright = await Playwright.CreateAsync();
            try
            {
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = headless
                });
            }
            catch (Exception ex)
            {
                var message = ex.Message ?? string.Empty;
                if (message.Contains("Executable doesn't exist") || message.Contains("playwright"))
                {
                    var help = "Playwright browser executables are missing. Run the install script to download browsers:\n" +
                               "  PowerShell: .\\Scripts\\install-playwright.ps1\n" +
                               "  or manually: pwsh -c \"dotnet tool install --global Microsoft.Playwright.CLI; playwright install\"";
                    throw new InvalidOperationException(help, ex);
                }

                throw;
            }
        }

        public static async Task<IPage> NewPageAsync()
        {
            if (_browser == null)
                throw new InvalidOperationException("Playwright is not initialized. Call InitAsync() first.");

            var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();
            return page;
        }

        public static async Task DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }

            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
        }
    }
}
