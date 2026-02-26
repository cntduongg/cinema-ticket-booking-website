# UnitTestSp3 - Booking Service Tests with Allure

This project contains unit tests for the Booking Service with Allure reporting integration.

## Prerequisites

- .NET 9.0 SDK
- Allure command line tool

## Installing Allure

### Windows (using Chocolatey)
```bash
choco install allure
```

### Windows (using Scoop)
```bash
scoop install allure
```

### Manual Installation
1. Download Allure from: https://github.com/allure-framework/allure2/releases
2. Extract to a directory
3. Add the bin directory to your PATH

## Running Tests with Allure

### 1. Run the tests and generate Allure results
```bash
dotnet test --logger "allure;allure.results.directory=allure-results"
```

### 2. Generate and open Allure report
```bash
allure serve allure-results
```

### 3. Generate static report (optional)
```bash
allure generate allure-results --clean
allure open
```

## Test Categories

The tests are organized into the following Allure categories:

### Features
- **Order Management**: Tests for creating, updating, and deleting orders
- **Booking Process**: Tests for the main booking workflow
- **Order Retrieval**: Tests for retrieving orders by various criteria

### Stories
- Create Order (with/without discount)
- Successful/Failed Booking scenarios
- Order retrieval (by ID, user ID, paged results)
- Order updates and deletions

### Severity Levels
- **Critical**: Core booking functionality and order creation
- **Normal**: Standard operations like updates, deletions, and error handling
- **Minor**: Retrieval operations and list functionality

### Tags
- `unit`: All unit tests
- `order`: Order-related operations
- `booking`: Booking process tests
- `create`, `update`, `delete`, `retrieve`: Specific operation types
- `success`, `failure`: Test outcome types

## Project Structure

```
UnitTestSp3/
├── BookingServiceTest.cs    # Main test file with Allure integration
├── UnitTestSp3.csproj       # Project file with Allure dependencies
└── README.md               # This file
```

## Allure Report Features

When you run the tests with Allure, you'll get:

1. **Test Overview**: Summary of all test results
2. **Feature-based Organization**: Tests grouped by functionality
3. **Severity-based Filtering**: Filter tests by importance level
4. **Detailed Test Information**: Step-by-step test execution
5. **Failure Analysis**: Detailed error information for failed tests
6. **Trends and Statistics**: Historical test performance data

## Troubleshooting

### Allure command not found
Make sure Allure is properly installed and added to your PATH.

### No test results generated
Ensure you're running the tests from the project directory and the `--logger` parameter is correctly specified.

### Report not opening
Try running `allure serve` instead of `allure open` for the web interface.

### Git ignore issues
The `allure-results/` and `TestResults/` directories are ignored by Git as they contain temporary test data. This is normal behavior.

## File Structure After Running Tests

After running tests, you should see:
```
UnitTestSp3/
├── allure-results/          # Allure test results (ignored by Git)
├── TestResults/             # MSTest results (ignored by Git)
├── BookingServiceTest.cs    # Test file
├── UnitTestSp3.csproj       # Project file
├── allure.runsettings       # Test configuration
├── .gitignore              # Git ignore rules
└── README.md               # This file
``` 