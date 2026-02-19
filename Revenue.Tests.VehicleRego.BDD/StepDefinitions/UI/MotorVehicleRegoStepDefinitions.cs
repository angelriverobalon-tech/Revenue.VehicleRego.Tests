using Revenue.Tests.VehicleRego.BDD.Model.Pages;
using Revenue.Tests.VehicleRego.BDD.Support;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Reqnroll;

namespace Revenue.Tests.BDD.StepDefinitions.UI
{
    [Binding]
    public sealed class MotorVehicleRegoStepDefinitions
    {
        private readonly IPage _page;
        private readonly ScenarioContext _scenarioContext;
        private VehicleRegistrationsPage? _vehicleRegistrationsPage;
        private DutyCalculatorPage? _dutyCalculatorPage;

        public MotorVehicleRegoStepDefinitions(IPage page, ScenarioContext scenarioContext)
        {
            _page = page;
            _scenarioContext = scenarioContext;
        }

        [Given("I am in the Check Motor Vehicle Stamp Duty page")]
        public async Task GivenIAmInTheCheckMotorVehicleStampDutyPageAsync()
        {
            // Use the injected page instead of creating a new one
            _vehicleRegistrationsPage = new VehicleRegistrationsPage(_page);
            _dutyCalculatorPage = new DutyCalculatorPage(_page);

            await _vehicleRegistrationsPage.NavigateToVehicleRegistrationsPage();
        }

        [When("I check online for the Motor Vehicle Registration")]
        public async Task WhenICheckOnlineForTheMotorVehicleRegistration()
        {
            if (_vehicleRegistrationsPage == null)
                throw new InvalidOperationException("Vehicle page not initialized");

            await _vehicleRegistrationsPage.ClickCheckOnlineButton();
        }

        [When("I register for a passenger vehicle")]
        public async Task WhenIRegisterForAPassengerVehicle()
        {
            if (_dutyCalculatorPage == null)
                throw new InvalidOperationException("Duty calculator page not initialized");

            await _dutyCalculatorPage.ClickYesPassengerVehicle();
        }

        [When(@"I calculate the '(.*)' for the vehicle")]
        public async Task WhenICalculateThePurchasePriceForTheVehicle(string purchasePrice)
        {
            if (_dutyCalculatorPage == null)
                throw new InvalidOperationException("Duty calculator page not initialized");

            await _dutyCalculatorPage.EnterPurchasePriceValue(purchasePrice);
            await _dutyCalculatorPage.ClickCalculateButton();
        }

        [Then(@"I should see calculated amount for the Motor Vehicle Registration with Duty payable as '(.*)'")]
        public async Task ThenIShouldSeeCalculatedAmountForTheMotorVehicleRegistrationWithDutyPayableAs(string expectedDuty)
        {
            if (_dutyCalculatorPage == null)
                throw new InvalidOperationException("Duty calculator page not initialized");

            var actualDuty = await _dutyCalculatorPage.GetDutyPayableNormalizedAsync();
            actualDuty.Should().Be(expectedDuty, $"Expected duty to be {expectedDuty}");
        }

        [Then("the Vehicle is a Passenger Vehicle")]
        public async Task ThenTheVehicleIsAPassengerVehicle()
        {
            if (_dutyCalculatorPage == null)
                throw new InvalidOperationException("Duty calculator page not initialized");

            var passengerVehicleText = await _dutyCalculatorPage.GetPassengerVehicleText();
            passengerVehicleText.Should().Be("Yes", "Vehicle should be a passenger vehicle");
        }
    }
}
