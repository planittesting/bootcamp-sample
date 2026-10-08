# Bootcamp Test Automation

This project is a .NET 10 test automation suite using:

- NUnit (unit/integration-style test suites)
- Reqnroll (BDD feature tests)
- Playwright for web UI automation
- REST API coverage against Swagger Petstore

## Project Structure

- `Test/Api` - NUnit API test suites
- `Test/Web` - NUnit UI test suites
- `BDD/Api` - Reqnroll API features and step definitions
- `BDD/Web` - Reqnroll web features and step definitions
- `Test/DataProviders` - NUnit data providers
- `Config` - environment and runtime config

## Prerequisites

- .NET SDK 10
- PowerShell (`pwsh`)
- Chrome (default Playwright channel in config)

## Setup

Restore dependencies:

```powershell
dotnet restore
```

Install Playwright browsers:

```powershell
pwsh bin/Debug/net10.0/playwright.ps1 install
```

## Running Tests

Run all tests:

```powershell
dotnet test
```

Run only NUnit API tests:

```powershell
dotnet test --filter "FullyQualifiedName~Bootcamp.Test.SwaggerPetStoreApiTests"
```

Run only NUnit UI tests:

```powershell
dotnet test --filter "FullyQualifiedName~Bootcamp.Test.JupiterPrimeUiTests"
```

## Configuration

- `Config/env.json` selects active environment (default: `dev`)
- `Config/appsettings.json` contains Playwright defaults
- `Config/appsettings.dev.json` contains base URLs and dev overrides

## Test Results and Reports

- `TestResults/` - runtime test artifacts (traces, videos, screenshots, etc.)
- `Reports/reqnroll_report.html` - Reqnroll HTML report
- Visual Studio Test Explorer also shows run results
