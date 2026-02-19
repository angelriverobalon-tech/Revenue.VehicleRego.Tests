using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Revenue.Tests.BDD.Model.Api;
using Revenue.Tests.BDD.Model.Api;
using System.Linq;

namespace Revenue.Tests.BDD.StepDefinitions.API
{
    [Binding]
    public sealed class GetAuthorStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private AuthorApi? _api;
        private AuthorDto? _author;

        public GetAuthorStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        // concrete simple API class
        private class AuthorApi : BaseApi<AuthorApi>
        {
        }

        [Given("the author ID \"(.*)\"")]
        public void GivenTheAuthorId(string authorId)
        {
            _scenarioContext["authorId"] = authorId;
        }

        [Given("I set the API base URL to \"(.*)\"")]
        public void GivenISetTheApiBaseUrl(string url)
        {
            _scenarioContext["API_BASE_URL"] = url;
        }

        [When("I request the author details")]
        public async Task WhenIRequestTheAuthorDetailsAsync()
        {
            string? url = null;

            if (_scenarioContext.ContainsKey("API_BASE_URL") && _scenarioContext["API_BASE_URL"] is string scUrl && !string.IsNullOrWhiteSpace(scUrl))
            {
                url = scUrl;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                var env = Environment.GetEnvironmentVariable("API_BASE_URL");
                if (!string.IsNullOrWhiteSpace(env))
                    url = env;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                url = Revenue.Tests.VehicleRego.BDD.Support.ConfigManager.GetConfigValue("API_BASE_URL", "https://openlibrary.org/authors/OL1A.json");
            }

            _api = new AuthorApi();
            await _api.GetAsync(url).ConfigureAwait(false);
        }

        [Then(@"the response status should be (\d+)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatus)
        {
            if (_api is null)
                throw new InvalidOperationException("API client not initialized");

            _api.GetStatusCode().Should().Be(expectedStatus);
        }

        [Then("the response JSON should contain \"(.*)\" with value \"(.*)\"")]
        public async Task ThenTheResponseJsonShouldContainWithValueAsync(string propertyName, string expectedValue)
        {
            if (_api is null)
                throw new InvalidOperationException("API client not initialized");

            _author = await _api.ReadJsonAsync<AuthorDto>().ConfigureAwait(false);
            if (_author is null)
                throw new Exception("Failed to deserialize response to AuthorDto");

            // support only the fields we care about for now
            if (string.Equals(propertyName, "personal_name", StringComparison.OrdinalIgnoreCase))
            {
                _author.PersonalName.Should().Be(expectedValue);
                return;
            }

            // fallback: reflectively check for string property
            var prop = typeof(AuthorDto).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, ToPascalCase(propertyName), StringComparison.OrdinalIgnoreCase));

            if (prop is null)
                throw new Exception($"AuthorDto does not contain property for '{propertyName}'");

            var val = prop.GetValue(_author)?.ToString();
            val.Should().Be(expectedValue);
        }

        [Then("the response JSON array \"(.*)\" should contain \"(.*)\"")]
        public async Task ThenTheResponseJsonArrayShouldContainAsync(string propertyName, string expected)
        {
            if (_api is null)
                throw new InvalidOperationException("API client not initialized");

            _author = await _api.ReadJsonAsync<AuthorDto>().ConfigureAwait(false);
            if (_author is null)
                throw new Exception("Failed to deserialize response to AuthorDto");

            if (string.Equals(propertyName, "alternate_names", StringComparison.OrdinalIgnoreCase))
            {
                (_author.AlternateNames ?? new System.Collections.Generic.List<string>()).Should().Contain(expected);
                return;
            }

            throw new NotSupportedException($"Array assertion for '{propertyName}' is not implemented");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _api?.Dispose();
            _api = null;
        }

        static string ToPascalCase(string snakeOrLower)
        {
            if (string.IsNullOrEmpty(snakeOrLower)) return snakeOrLower;
            // handle snake_case or lower_with_underscores
            var parts = snakeOrLower.Split('_');
            return string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1)));
        }
    }
}
