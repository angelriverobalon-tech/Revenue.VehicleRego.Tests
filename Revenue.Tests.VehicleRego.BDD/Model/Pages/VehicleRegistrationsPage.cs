using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revenue.Tests.VehicleRego.BDD.Support;

namespace Revenue.Tests.VehicleRego.BDD.Model.Pages
{
    public class VehicleRegistrationsPage : BasePage<VehicleRegistrationsPage>
    {
        public VehicleRegistrationsPage()
        {
        }

        public VehicleRegistrationsPage(IPage page) : base(page)
        {
            this.page = page;
        }

        #region locators
        string checkOnlineButton = "//a[normalize-space()='Check online']";
        #endregion

        #region methods

        public async Task NavigateToVehicleRegistrationsPage()
        {
            if (page == null)
                throw new InvalidOperationException("Page is not initialized");

            var url = ConfigManager.GetConfigValue("PLAYWRIGHT_URL");

            // Use DOMContentLoaded - much faster than NetworkIdle
            await page.GotoAsync(url, new PageGotoOptions
            {
                Timeout = 20000,
                WaitUntil = WaitUntilState.DOMContentLoaded  // FAST - don't wait for all network
            });

            // Quick cookie dismiss (1 second max)
            await QuickDismissCookies();
        }

        private async Task QuickDismissCookies()
        {
            if (page == null) return;

            try
            {
                var acceptButton = page.Locator("button:has-text('Accept')");
                await acceptButton.ClickAsync(new() { Timeout = 1000 });
            }
            catch
            {
                // No cookie banner or timed out - continue
            }
        }

        public async Task<VehicleRegistrationsPage> ClickCheckOnlineButton()
        {
            if (page == null)
                throw new InvalidOperationException("Page is not initialized");

            // Simple, fast click
            await page.Locator(checkOnlineButton).ClickAsync(new()
            {
                Timeout = 10000  // 10 seconds max
            });

            // Wait for next page to start loading (not fully loaded)
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new() { Timeout = 10000 });

            return this;
        }

        #endregion

    }
}
