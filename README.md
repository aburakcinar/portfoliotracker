# Portfolio Tracker

A personal finance management tool to track and manage investment portfolios.

## Features

- **Track Holdings**: Add and manage individual holdings (stocks, funds, etc.)
- **Search Assets**: Search through assets with filtering capabilities
- **Performance Tracking**: View portfolio performance metrics
- **Reporting**: Generate reports on portfolio activity and returns
- **RESTful API**: Programmatic access to portfolio data via HTTP endpoints

## Installation

1. Clone the repository:

```bash
git clone https://github.com/aburakcinar/portfoliotracker.git
```

2. Restore NuGet packages:

```bash
dotnet restore
```

3. Configure your database (if needed):

```bash
dotnet ef migrations configure --context PortfolioContext
```

4. Run the application:

```bash
dotnet run
```

## Usage

The application can be accessed via:

- Web UI at `http://localhost:5000`
- REST API at `http://localhost:5255`

For API documentation, visit `http://localhost:5255/swagger`.

### Example API Call

To search assets:

```bash
curl -X GET "http://localhost:5001/api/assets?searchText=example"
```

## License

MIT License
