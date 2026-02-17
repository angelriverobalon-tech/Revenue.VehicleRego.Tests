# Revenue.Tests
This repository provides a comprehensive test framework for verifying the end‑to‑end vehicle registration process (UI) and Author Retrieval from Open Library (API).


# Environment setup
This project is incomplete, it is intended for assessment purposes.

During development to provide environment variables to the solution create a file named env.json at the root of the project folder. For example:
```json
{
  "PLAYWRIGHT_URL": "https://www.service.nsw.gov.au/transaction/check-motor-vehicle-stamp-duty",
  "PLAYWRIGHT_HEADLESS": "false",
  "PLAYWRIGHT_BROWSER": "chromium",
  "API_BASE_URL": "https://openlibrary.org/authors/OL1A.json"
}
```
**Do NOT include this file in source control**
1. Restore packages and build:
`dotnet restore`
`dotnet build`
2. Install playwright with using Powershell `.\Scripts\install-playwright.ps1`

# Build and Test

Run the following command to build the solution:

`dotnet build`

And to test:

`dotnet test`

# Run individual Reqnroll Tests

Run individual Reqnroll tests from the associated .feature.cs file

# Api

The Api tests use the RestSharp library
