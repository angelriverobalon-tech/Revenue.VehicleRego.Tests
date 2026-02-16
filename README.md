# Revenue.VehicleRego.Tests
This repository provides a comprehensive test framework for verifying the end‑to‑end vehicle registration process


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

# Build and Test

Run the following command to build the solution:

`dotnet build`

And to test:

`dotnet test`

# "Specflow" Plugin for VsCode

At time of writing there is no official plugin for specflow with VsCode.

However, you can use the Cucumber plugin and modify it for our Specflow needs, the guide is here:

https://docs.specflow.org/projects/specflow/en/latest/vscode/vscode-specflow.html

# Run individual SpecFlow Tests

Run individual specflow tests from the associated .feature.cs file

# Api

The Api tests use the RestSharp library
