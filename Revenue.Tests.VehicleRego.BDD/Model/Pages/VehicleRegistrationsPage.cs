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

        public async Task<VehicleRegistrationsPage> NavigateToVehicleRegistrationsPage()
        {
            var defaultUrl = "https://www.revenue.nsw.gov.au/vehicle-registrations";

            var url = ConfigManager.GetConfigValue("PLAYWRIGHT_URL", defaultUrl);

            await page.GotoAsync(url);
            return this;
        }
        public async Task<VehicleRegistrationsPage> ClickCheckOnlineButton()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            await page.Locator(checkOnlineButton).ClickAsync();
            return this;
        }

        #endregion

    }
}
