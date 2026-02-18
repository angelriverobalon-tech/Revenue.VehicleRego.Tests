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
                    Headless = headless,
                    // Speed optimizations
                    Args = new[]
                    {
                        "--disable-blink-features=AutomationControlled",
                        "--disable-dev-shm-usage",
                        "--no-sandbox"
                    }
                });
            }
            catch (Exception ex)
            {
                var message = ex.Message ?? string.Empty;
                if (message.Contains("Executable doesn't exist") || message.Contains("playwright"))
                {
                    var help = "Playwright browser executables are missing. Run:\n" +
                               "  pwsh -c \"dotnet tool install --global Microsoft.Playwright.CLI; playwright install chromium\"";
                    throw new InvalidOperationException(help, ex);
                }
                throw;
            }
        }

        public static async Task<IPage> NewPageAsync()
        {
            if (_browser == null)
                throw new InvalidOperationException("Playwright is not initialized. Call InitAsync() first.");

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                Locale = "en-AU",
                TimezoneId = "Australia/Sydney",

                // Disable unnecessary features for speed
                AcceptDownloads = false,
                HasTouch = false,
                IsMobile = false,
                JavaScriptEnabled = true
            });

            // Reasonable timeouts - not too long
            context.SetDefaultTimeout(15000); // 15 seconds
            context.SetDefaultNavigationTimeout(20000); // 20 seconds

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
