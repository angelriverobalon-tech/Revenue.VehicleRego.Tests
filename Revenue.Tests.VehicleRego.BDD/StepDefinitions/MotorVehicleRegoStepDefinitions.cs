using Revenue.Tests.VehicleRego.BDD.Model.Pages;
using Revenue.Tests.VehicleRego.BDD.Support;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using System.Text.RegularExpressions;

namespace Revenue.Tests.VehicleRego.BDD.StepDefinitions
{
    [Binding]
    public sealed class MotorVehicleRegoStepDefinitions
    {
        private VehicleRegistrationsPage? _vehicleRegistrationsPage;
        private DutyCalculatorPage? _dutyCalculatorPage;
        private readonly ScenarioContext _scenarioContext;

        public MotorVehicleRegoStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I am in the Check Motor Vehicle Stamp Duty page")]
        public async Task GivenIAmInTheCheckMotorVehicleStampDutyPageAsync()
        {
            var headlessStr = ConfigManager.GetConfigValue("PLAYWRIGHT_HEADLESS", "true");
            var headless = !string.Equals(headlessStr, "false", StringComparison.OrdinalIgnoreCase);

            await PlaywrightDriver.InitAsync(headless);
            var page = await PlaywrightDriver.NewPageAsync();

            _vehicleRegistrationsPage = new VehicleRegistrationsPage(page);
            _dutyCalculatorPage = new DutyCalculatorPage(page);

            await _vehicleRegistrationsPage.NavigateToVehicleRegistrationsPage();
        }

        [When("I check online for the Motor Vehicle Registration")]
        public async Task WhenICheckOnlineForTheMotorVehicleRegistration()
        {
            await _vehicleRegistrationsPage.ClickCheckOnlineButton();
        }

        [When("I register for a passenger vehicle")]
        public async Task WhenIRegisterForAPassengerVehicle()
        {
            await _dutyCalculatorPage.ClickYesPassengerVehicle();
        }

        [When("I calculate the {string} for the vehicle")]
        public async Task WhenICalculateTheForTheVehicle(string purchasePrice)
        {
            _scenarioContext["purchasePrice"] = purchasePrice;
            await _dutyCalculatorPage.EnterPurchasePriceValue(purchasePrice);
            await _dutyCalculatorPage.ClickCalculateButton();
        }

        [Then("I should see calculated amount for the Motor Vehicle Registration with Duty payable as {string}")]
        public async Task ThenIShouldSeeCalculatedAmountForTheMotorVehicleRegistrationWithDutyPayableAs(string dutyPayable)
        {
            string purchasePrice = _scenarioContext["purchasePrice"] as string ?? string.Empty;
            var expectedDutyPayableNormalised = DutyCalculatorPage.NormalizeCurrencyStringPublic(dutyPayable);
            var actualDutyPayableNormalised = await _dutyCalculatorPage.GetDutyPayableNormalizedAsync();

            var expectedPurchaseNormalized = DutyCalculatorPage.NormalizeCurrencyStringPublic(purchasePrice);
            var actualPurchaseNormalized = await _dutyCalculatorPage.GetPurchasePriceNormalisedAsync();

            actualDutyPayableNormalised.Should().Be(expectedDutyPayableNormalised, $"Expected duty payable to be {expectedDutyPayableNormalised} but was {actualDutyPayableNormalised}");
            actualPurchaseNormalized.Should().Be(expectedPurchaseNormalized, $"Expected Purchase Price to be {expectedPurchaseNormalized} but was {actualPurchaseNormalized}");
        }

        [Then("the Vehicle is a Passenger Vehicle")]
        public async Task ThenTheVehicleIsAPassengerVehicle()
        {
            var actual = await _dutyCalculatorPage.GetPassengerVehicleText();
            var cleaned = CleanText(actual);
            cleaned.Should().Be("Yes");
        }

        string CleanText(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            // unescape JSON-style escapes like \<\/td\> -> </td>
            raw = Regex.Unescape(raw);

            // remove any HTML tags
            raw = Regex.Replace(raw, "<.*?>", string.Empty);

            // decode HTML entities if present (&amp; etc.)
            raw = WebUtility.HtmlDecode(raw);

            return raw.Trim();
        }
    }
}
