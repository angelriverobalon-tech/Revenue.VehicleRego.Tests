using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Revenue.Tests.VehicleRego.BDD.Support;

namespace Revenue.Tests.VehicleRego.BDD.Model.Pages
{
    public class DutyCalculatorPage : BasePage<DutyCalculatorPage>
    {
        public DutyCalculatorPage()
        {
        }

        public DutyCalculatorPage(IPage page) : base(page)
        {
            this.page = page;
        }

        private IPage? _popupPage;

        #region locators
        string yesPassengerVehicleRadioButton = "//label[normalize-space()='Yes']";
        string purchasePriceInput = "//input[@id='purchasePrice']";
        string calculateButton = "//button[normalize-space()='Calculate']";
        string calculatePopUpWindow = "//h4[normalize-space()='Calculation']";
        string motorVehicleRegistrationHeader = "//h4[normalize-space()='Motor vehicle registration']";
        string isThisRegoForAPassengerVehicleTextLabel = "//td[normalize-space()='Is this registration for a passenger vehicle?']";
        string passengerVehicleText = "table[class*='TableApp'] > tbody > tr:nth-of-type(2) > td:nth-of-type(2)";
        string purchasePriceOrValueTextLabel = "//td[normalize-space()='Purchase price or value']";
        string purchasePriceOrValueText = "div > table > tbody > tr:nth-child(3) > td.focus.right";
        string dutyPayableTextLabel = "//td[normalize-space()='Duty payable']";
        string dutyPayableText = "div > table > tbody > tr:nth-child(5) > td.focus.right";
        string closeButton = "//button[normalize-space()='Close']";


        #endregion

        #region methods

        public static string NormalizeCurrencyStringPublic(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var cultures = new[] { CultureInfo.GetCultureInfo("en-AU"), CultureInfo.GetCultureInfo("en-US"), CultureInfo.InvariantCulture };
            foreach (var c in cultures)
            {
                if (decimal.TryParse(input, NumberStyles.Currency | NumberStyles.Number, c, out var value))
                    return value.ToString("0.00", CultureInfo.InvariantCulture);
            }

            var cleaned = new string(input.Where(ch => char.IsDigit(ch) || ch == '.' || ch == '-').ToArray());
            if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var fallback))
                return fallback.ToString("0.00", CultureInfo.InvariantCulture);

            return input.Trim();
        }

        public async Task<string> GetDutyPayableNormalizedAsync()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");

            // 1) If a popup page was opened by the calculate action, prefer its content
            if (_popupPage != null)
            {
                try
                {
                    var loc = _popupPage.Locator($"xpath=//h4[normalize-space()='Calculation']/following::table[1]//tr[td[normalize-space()='Duty payable']]//td[contains(@class,'focus') or contains(@class,'right')]");
                    if (await loc.CountAsync() > 0)
                    {
                        var raw = (await loc.First.InnerTextAsync()).Trim();
                        return NormalizeCurrencyStringPublic(raw);
                    }

                    var popupHtml = await _popupPage.ContentAsync();
                    var m = Regex.Match(popupHtml, @"Duty payable[\s\S]{0,200}?\$?([0-9\,]+\.?[0-9]{0,2})", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        return NormalizeCurrencyStringPublic(m.Groups[1].Value);
                    }
                }
                catch
                {
                    // ignore and fallback to main page
                }
            }

            // 2) Try to read from in-page modal/table
            try
            {
                var locator = page.Locator($"xpath=//h4[normalize-space()='Calculation']/following::table[1]//tr[td[normalize-space()='Duty payable']]//td[contains(@class,'focus') or contains(@class,'right')]");
                if (await locator.CountAsync() > 0)
                {
                    var raw = (await locator.First.InnerTextAsync()).Trim();
                    return NormalizeCurrencyStringPublic(raw);
                }
            }
            catch { }

            // 3) Scan current page HTML
            try
            {
                var html = await page.ContentAsync();
                var m = Regex.Match(html, @"Duty payable[\s\S]{0,200}?\$?([0-9\,]+\.?[0-9]{0,2})", RegexOptions.IgnoreCase);
                if (m.Success)
                    return NormalizeCurrencyStringPublic(m.Groups[1].Value);
            }
            catch { }

            // 4) fallback to label locator
            var fallbackRaw = (await page.Locator(dutyPayableText).InnerTextAsync()).Trim();
            return NormalizeCurrencyStringPublic(fallbackRaw);
        }

        public async Task<DutyCalculatorPage> ClickYesPassengerVehicle()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            await page.Locator(yesPassengerVehicleRadioButton).ClickAsync();
            return this;
        }
        public async Task<DutyCalculatorPage> EnterPurchasePriceValue(string purchasePrice)
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            await page.Locator(purchasePriceInput).FillAsync(purchasePrice);
            return this;
        }

        public async Task<DutyCalculatorPage> ClickCalculateButton()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            await page.Locator(calculateButton).ClickAsync();
            return this;
        }

        public async Task<bool> IsCalculatePopUpWindowDisplayed()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            return await page.Locator(calculatePopUpWindow).IsVisibleAsync();
        }

        public async Task<bool> IsMotorVehicleRegistrationHeaderDisplayed()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            return await page.Locator(motorVehicleRegistrationHeader).IsVisibleAsync();
        }

        public async Task<bool> IsThisRegoForAPassengerVehicleTextLabelDisplayed()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            return await page.Locator(isThisRegoForAPassengerVehicleTextLabel).IsVisibleAsync();
        }

        public async Task<string> GetPassengerVehicleText()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            var passengerVehicleTextValue = await page.Locator(passengerVehicleText).InnerTextAsync();
            return passengerVehicleTextValue?.Trim() ?? string.Empty;
        }

        public async Task<bool> IsPurchasePriceOrValueTextLabelDisplayed()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            return await page.Locator(purchasePriceOrValueTextLabel).IsVisibleAsync();
        }

        public async Task<string> GetPurchasePriceNormalisedAsync()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");

            // 1) If a popup page was opened by the calculate action, prefer its content
            if (_popupPage != null)
            {
                try
                {
                    var loc = _popupPage.Locator($"xpath=//h4[normalize-space()='Calculation']/following::table[1]//tr[td[normalize-space()='Purchase price or value']]//td[contains(@class,'focus') or contains(@class,'right')]");
                    if (await loc.CountAsync() > 0)
                    {
                        var raw = (await loc.First.InnerTextAsync()).Trim();
                        return NormalizeCurrencyStringPublic(raw);
                    }

                    var popupHtml = await _popupPage.ContentAsync();
                    var m = Regex.Match(popupHtml, @"Purchase price or value[\s\S]{0,200}?\$?([0-9\,]+\.?[0-9]{0,2})", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        return NormalizeCurrencyStringPublic(m.Groups[1].Value);
                    }
                }
                catch
                {
                    // ignore and fallback to main page
                }
            }

            // 2) Try to read from in-page modal/table
            try
            {
                var locator = page.Locator($"xpath=//h4[normalize-space()='Calculation']/following::table[1]//tr[td[normalize-space()='Purchase price or value']]//td[contains(@class,'focus') or contains(@class,'right')]");
                if (await locator.CountAsync() > 0)
                {
                    var raw = (await locator.First.InnerTextAsync()).Trim();
                    return NormalizeCurrencyStringPublic(raw);
                }
            }
            catch { }

            // 3) Scan current page HTML
            try
            {
                var html = await page.ContentAsync();
                var m = Regex.Match(html, @"Purchase price or value[\s\S]{0,200}?\$?([0-9\,]+\.?[0-9]{0,2})", RegexOptions.IgnoreCase);
                if (m.Success)
                    return NormalizeCurrencyStringPublic(m.Groups[1].Value);
            }
            catch { }

            // 4) fallback to label locator
            var fallbackRaw = (await page.Locator(purchasePriceOrValueText).InnerTextAsync()).Trim();
            return NormalizeCurrencyStringPublic(fallbackRaw);
        }

        public async Task<bool> IsDutyPayableTextDisplayed()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            return await page.Locator(dutyPayableTextLabel).IsVisibleAsync();
        }

        public async Task<bool> IsDutyPayableTextCorrect(string dutyPayable)
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            // Get Duty Payable from the UI and normalize it for comparison
            string actualRaw = (await page.Locator(dutyPayableText).InnerTextAsync()).Trim();
            static string NormalizeCurrencyString(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    return string.Empty;
                // Try parsing as currency using common cultures
                var cultures = new[] { CultureInfo.GetCultureInfo("en-AU"), CultureInfo.GetCultureInfo("en-US"), CultureInfo.InvariantCulture };
                foreach (var c in cultures)
                {
                    if (decimal.TryParse(input, NumberStyles.Currency | NumberStyles.Number, c, out var value))
                        return value.ToString("0.00", CultureInfo.InvariantCulture);
                }
                // Fallback: strip non-digit except dot and minus then parse
                var cleaned = new string(input.Where(ch => char.IsDigit(ch) || ch == '.' || ch == '-').ToArray());
                if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var fallback))
                    return fallback.ToString("0.00", CultureInfo.InvariantCulture);
                return input.Trim();
            }
            var actualNormalized = NormalizeCurrencyString(actualRaw);
            var expectedNormalized = NormalizeCurrencyString(dutyPayable);
            return string.Equals(actualNormalized, expectedNormalized, StringComparison.Ordinal);
        }

        public async Task<DutyCalculatorPage> ClickCloseButton()
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");
            await page.Locator(closeButton).ClickAsync();
            return this;
        }

        /// <summary>
        /// Click the calculate button and return the page that contains the result (main page or popup)
        /// together with the normalized duty payable string (canonical "0.00" format).
        /// </summary>
        public async Task<(IPage resultPage, string normalizedDuty)> ClickCalculateAndGetDutyAsync(int popupTimeoutMs = 5000, int modalTimeoutMs = 5000)
        {
            if (page == null) throw new InvalidOperationException("Page is not initialized");

            // Try to detect a popup opened by the click using RunAndWaitForPopupAsync
            try
            {
                IPage popup = null!;
                try
                {
                    popup = await page.RunAndWaitForPopupAsync(async () =>
                    {
                        await page.Locator(calculateButton).ClickAsync();
                    }, new PageRunAndWaitForPopupOptions { Timeout = popupTimeoutMs });
                }
                catch
                {
                    // no popup detected within timeout or RunAndWaitForPopupAsync threw
                    popup = null!;
                }

                if (popup != null)
                {
                    _popupPage = popup;
                    try { await popup.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = modalTimeoutMs }); } catch { }
                    // extract duty from popup
                    var duty = await ExtractDutyFromPageAsync(popup);
                    return (popup, duty);
                }
            }
            catch
            {
                // swallow and fallback to main page
            }

            // No popup: click and wait for in-page modal
            await page.Locator(calculateButton).ClickAsync();
            try { await page.WaitForSelectorAsync(calculatePopUpWindow, new PageWaitForSelectorOptions { Timeout = modalTimeoutMs }); } catch { }

            var dutyMain = await ExtractDutyFromPageAsync(page);
            return (page, dutyMain);
        }

        private async Task<string> ExtractDutyFromPageAsync(IPage p)
        {
            // Try table locator first
            try
            {
                var loc = p.Locator($"xpath=//h4[normalize-space()='Calculation']/following::table[1]//tr[td[normalize-space()='Duty payable']]//td[contains(@class,'focus') or contains(@class,'right')]");
                if (await loc.CountAsync() > 0)
                {
                    var raw = (await loc.First.InnerTextAsync()).Trim();
                    return NormalizeCurrencyStringPublic(raw);
                }
            }
            catch { }

            // Regex on HTML
            try
            {
                var html = await p.ContentAsync();
                var m = Regex.Match(html, @"Duty payable[\s\S]{0,200}?\$?([0-9\,]+\.?[0-9]{0,2})", RegexOptions.IgnoreCase);
                if (m.Success)
                    return NormalizeCurrencyStringPublic(m.Groups[1].Value);
            }
            catch { }

            // Fallback to label locator
            try
            {
                var fallbackRaw = (await p.Locator(dutyPayableText).InnerTextAsync()).Trim();
                return NormalizeCurrencyStringPublic(fallbackRaw);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

    }
}
